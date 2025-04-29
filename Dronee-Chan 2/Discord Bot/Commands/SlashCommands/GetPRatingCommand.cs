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
    internal class GetPRatingCommand : ApplicationCommandModule
    {
        [Command("GetPRating")]
        [RequireRolesSlash(RoleCheckMode.Any, "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task GetPRating(CommandContext ctx, [Option("ID", "The discord UUID of the person")] string SID)
        {
            await ctx.DeferResponseAsync();

            string response = await PRatingEvents.GetPRating(ulong.Parse(SID));

            await ctx.EditResponseAsync("Current PRating of this person is: " + response);
        }
    }
}
