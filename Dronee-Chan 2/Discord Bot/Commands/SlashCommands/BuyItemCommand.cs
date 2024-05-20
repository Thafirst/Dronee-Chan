using Dronee_Chan_2.Discord_Bot.Attributes;
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
    internal class BuyItemCommand : ApplicationCommandModule
    {
        [SlashCommand("BuyItem", "Buys the item with the supplied ID")]
        [RequireRolesSlash(RoleCheckMode.Any, "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task BuyItem(InteractionContext ctx, [Option("Name", "The Name of the item you wish to buy", true)]string item, [Option("Amount", "The amount of the item you want", false)]long LAmount)
        {
            await ctx.DeferAsync();

            int amount = (int)LAmount;

            item = item.TrimEnd(' ');

            ulong id = ctx.User.Id;


            if (!Program.IM.isID(item))
            {
                item = Program.IM.getIdFromName(item);
                if (item == "")
                {
                    await Context.Message.ReplyAsync("I could not find the item with the given ID or name.");
                    return;
                }
            }

            int price = (Program.IM.getItemPrice(item)) * amount;

            if (!(Program.CurrencyList[id] >= price))
            {
                await Context.Message.ReplyAsync("You cannot afford " + amount + "x " + Program.IM.getItemFromID(item).name + ", it cost R$" + price);
                return;
            }


            bool success = Program.IM.addToInventory(id, item, amount);
            if (!success)
            {
                await Context.Message.ReplyAsync("Something went wrong while trying to add the item, maybe they dont have space for the items.");
                return;
            }
            Program.CurrencyList[id] -= (Program.IM.getItemPrice(item) * amount);
            await Context.Message.ReplyAsync(amount + "x " + Program.IM.getItemFromID(item).name + " has been added to " + Context.Guild.GetUser(id).Username + "(-R$" + Program.IM.getItemPrice(item) * amount + ")");
            saveInventories();
            Program.MDM.saveCurrencyDictionary();

            User user = Program.Users[id];
            user.setCurrency(user.getCurrency() - (Program.IM.getItemPrice(item) * amount));
            await Program.PC.saveUsers(user);

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Pong"));
        }

        private Item getItem(string name)
        {
            
        }
    }
}
