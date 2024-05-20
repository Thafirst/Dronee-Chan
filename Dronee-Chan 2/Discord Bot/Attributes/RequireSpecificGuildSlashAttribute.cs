using DSharpPlus.CommandsNext;
using DSharpPlus.SlashCommands;
using System.Collections.ObjectModel;

namespace Dronee_Chan_2.Discord_Bot.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    internal class RequireSpecificGuildSlashAttribute : SlashCheckBaseAttribute
    {
        public IReadOnlyList<ulong> GuildIds { get; }
        public GuildCheckMode CheckMode { get; }
        public RequireSpecificGuildSlashAttribute(GuildCheckMode checkMode, params ulong[] ids)
        {
            CheckMode = checkMode;
            GuildIds = new ReadOnlyCollection<ulong>(ids);
        }

        public override Task<bool> ExecuteChecksAsync(InteractionContext ctx)
        {
            bool contains = GuildIds.Contains(ctx.Guild.Id);
            return CheckMode switch
            {
                GuildCheckMode.Any => Task.FromResult(contains),
                GuildCheckMode.None => Task.FromResult(!contains),
                _ => Task.FromResult(false),
            };
        }
    }
}
