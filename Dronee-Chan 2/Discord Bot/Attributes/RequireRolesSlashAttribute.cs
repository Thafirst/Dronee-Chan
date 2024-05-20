using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System.Collections.ObjectModel;

namespace Dronee_Chan_2.Discord_Bot.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    internal class RequireRolesSlashAttribute : SlashCheckBaseAttribute
    {
        public IReadOnlyList<string> RoleNames { get; }
        public RoleCheckMode CheckMode { get; }
        public RequireRolesSlashAttribute(RoleCheckMode checkMode, params string[] roleNames)
        {
            CheckMode = checkMode;
            RoleNames = new ReadOnlyCollection<string>(roleNames);
        }

        public override Task<bool> ExecuteChecksAsync(InteractionContext ctx)
        {
            if (ctx.Guild == null || ctx.Member == null)
            {
                return Task.FromResult(result: false);
            }

            var count = (RoleNames.Except(ctx.Member.Roles.Select((DiscordRole xm) => xm.Name))).Count();
            return CheckMode switch
            {
                RoleCheckMode.All => Task.FromResult(count == 0),
                RoleCheckMode.Any => Task.FromResult(count != RoleNames.Count),
                RoleCheckMode.SpecifiedOnly => Task.FromResult(RoleNames.SequenceEqual(ctx.Member.Roles.Select((DiscordRole xm) => xm.Name))),
                RoleCheckMode.None => Task.FromResult(count == RoleNames.Count),
                _ => Task.FromResult(count > 0),
            };
        }


    }
}
