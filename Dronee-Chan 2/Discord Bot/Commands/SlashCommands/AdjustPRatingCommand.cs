using Dronee_Chan_2.Discord_Bot.Attributes;
using Dronee_Chan_2.Discord_Bot.Events;
using DSharpPlus.Commands;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Commands.SlashCommands
{
    internal class AdjustPRatingCommand : ApplicationCommandModule
    {
        [Command("AdjustPRating")]
        [RequireRolesSlash(RoleCheckMode.Any, "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task AdjustPRating(CommandContext ctx, [Option("ID", "The discord UUID of the person")] string SID)
        {
            await ctx.DeferResponseAsync();

            PRatingEvents.EditPRating(ulong.Parse(SID));
        }
    }
}
