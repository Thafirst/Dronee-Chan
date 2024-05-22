using Dronee_Chan_2.Discord_Bot.Attributes;
using Dronee_Chan_2.Discord_Bot.Events;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Commands.SlashCommands
{
    internal class GiveBondsCommand : ApplicationCommandModule
    {
        [SlashCommand("GiveBonds", "Replies with Pong")]
        [RequireRolesSlash(RoleCheckMode.Any, "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task GiveBonds(InteractionContext ctx, [Option("Type","Type of bonds to give, C or T.", false)]string Type,
                                                            [Option("Amount","The amount of bonds to give.")] long LAmount,
                                                            [Option("UUID", "The Discord UUID of the person to recieve the bonds.")] string SID)
        {
            await ctx.DeferAsync(true);

            var value = await EventManager.GetBonds(char.Parse(Type));

            ulong ID = ulong.Parse(SID);

            int amount = (int) LAmount;

            if(!await EventManager.Pay(value * amount, ID))
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Something went wrong while trying to payout the bonds."));
                return;
            }

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(ctx.Guild.GetMemberAsync(ID).Result.Username + " has been paid."));
        }
    }
}
