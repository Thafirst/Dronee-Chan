using Dronee_Chan_2.Discord_Bot.Attributes;
using DSharpPlus.Commands;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Commands.SlashCommands
{
    internal class RanksCommand : ApplicationCommandModule
    {
        [Command("Ranks")]
        [RequireRolesSlash(RoleCheckMode.Any, "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task Ranks(CommandContext ctx)
        {
            await ctx.DeferResponseAsync();

            string message = "__**Available ID Card Ranks are as follows:**__\r\n\r\nCivilian (1 - 10)\r\nRecruit (11 - 20)\r\nEnlisted (21 - 30)\r\nMercenary (31 - 40)\r\nProfessional (41 - 50)\r\nVeteran (51 - 60)\r\nAce (61 - 70)\r\nWarlord (71 - 80)\r\nElite (81 - 90)\r\nLegend (91 - 100)\r\nMyth (101)";

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(message));
        }
    }
}
