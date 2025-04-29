using Dronee_Chan_2.Discord_Bot.Attributes;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Commands.SlashCommands
{
    internal class PRatingCommand : ApplicationCommandModule
    {
        [Command("PRating")]
        [RequireRolesSlash(RoleCheckMode.Any, "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task PRating(CommandContext ctx)
        {
            await ctx.DeferResponseAsync();

            await ctx.EditResponseAsync("We do not disclose details about how the P.Rating System works, and we will not " +
                "discuss its mechanics. The best approach is to focus on flying well, putting in effort, and improving " +
                "your skills. If you have concerns about your rating, concentrate on consistent performance rather than " +
                "trying to figure out the system.\r\n\r\nPlease see the FAQ regarding P.Rating below for more information:" +
                "\r\n\r\n" + ctx.Guild.GetChannelAsync(782700981158674443).Result.GetMessageAsync(1356707091808780339).Result.JumpLink);
        }
    }
}
