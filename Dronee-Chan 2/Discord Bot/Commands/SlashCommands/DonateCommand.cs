using Dronee_Chan_2.Discord_Bot.Attributes;
using Dronee_Chan_2.Discord_Bot.Events;
using Dronee_Chan_2.Discord_Bot.Objects.UserObjects;
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
    internal class DonateCommand : ApplicationCommandModule
    {
        [SlashCommand("Donate", "Donates R$ to the PMC in order to increase your rank.")]
        [RequireRolesSlash(RoleCheckMode.Any, "Member", "Staff", "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task Donate(InteractionContext ctx, [Option("Amount", "Amount that you wish to donate to the PMC", true)] string SAmount)
        {
            await ctx.DeferAsync();

            int amount = 0;

            if(SAmount.ToLower() != "all")
            {
                try
                {
                    amount = int.Parse(SAmount);

                    if (amount <= 0)
                    {
                        await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("You cannot donate 0 or less R$."));
                        return;
                    }
                }
                catch(Exception) 
                {
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Could not parse the amount supplied to \"all\" or a number."));
                    return;
                }
            }

            User user = await EventManager.LoadUserEvent(ctx.User.Id);

            if (user.Currency <= 0)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("You do not have enough R$ to donate."));
                return;
            }

            if (amount == 0)
            {
                amount = user.Currency;
            }

            if (user.Currency < amount)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("You cannot donate more R$ than you own."));
                return;
            }
            user.DonationAmount += amount;
            user.Currency -= amount;

            EventManager.SaveUserEvent(user);



            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Thank you for your donation to the PMC, we appreciate it, you donated " + amount));
        }
    }
}
