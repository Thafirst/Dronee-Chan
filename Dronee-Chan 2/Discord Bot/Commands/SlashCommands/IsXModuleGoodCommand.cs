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
    internal class IsXModuleGoodCommand : ApplicationCommandModule
    {
        [Command("IsXModuleGood")]
        [RequireRolesSlash(RoleCheckMode.Any, "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task IsXModuleGood(CommandContext ctx)
        {
            await ctx.DeferResponseAsync();

            string message = "__**\"Is X module good ? \" / \"Should I buy X module ? \"**__ \r\n\r\nThere is never a correct " +
                "answer to this question that anybody other than yourself can give - think about what it is that you want to " +
                "do in DCS, and especially what kind of aircraft you like, and base your decisions from that and some research." +
                "\r\n\r\nThere is also the ability to trial modules individually for two weeks on the standalone version of DCS, " +
                "which we highly recommend.";

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(message));
        }
    }
}
