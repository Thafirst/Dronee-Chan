using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    internal class RequireSpecificGuildAttribute : CheckBaseAttribute
    {
        public IReadOnlyList<ulong> GuildIds { get; }
        public GuildCheckMode CheckMode { get; }
        public RequireSpecificGuildAttribute(GuildCheckMode checkMode, params ulong[] ids)
        {
            CheckMode = checkMode;
            GuildIds = new ReadOnlyCollection<ulong>(ids);
        }
        public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
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
