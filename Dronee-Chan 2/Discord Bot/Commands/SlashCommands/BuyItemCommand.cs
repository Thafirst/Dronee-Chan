using Dronee_Chan_2.Discord_Bot.Attributes;
using Dronee_Chan_2.Discord_Bot.Events;
using Dronee_Chan_2.Discord_Bot.Objects.UserObjects;
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
    internal class BuyItemCommand : ApplicationCommandModule
    {
        [Command("BuyItem")]
        [RequireRolesSlash(RoleCheckMode.Any, "Member", "Staff", "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task BuyItem(CommandContext ctx, [Option("Name", "The Name of the item you wish to buy", true)]string item, [Option("Amount", "The amount of the item you want", false)]long LAmount = 1)
        {
            await ctx.DeferResponseAsync();

            int amount = (int)LAmount;

            ulong id = ctx.User.Id;

            var Item = await EventManager.GetItem(item);

            if (Item == null)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("I could not find the item with the given name."));
                return;
            }

            User user = await EventManager.LoadUser(id);

            if(user.Currency < Item.BuyPrice * amount)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("You cannot afford " + amount + "x " + Item.Name + ", it costs R$" + Item.BuyPrice * amount));
                return;
            }

            if(user.Inventory.Count > 15 - amount)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("You do not have enough inventory slots for " + amount + " items."));
                return;
            }

            for(int i = 0; i < amount; i++)
                user.AddToInventory(Item.ID.ToString());
            user.Currency -= (amount * Item.BuyPrice);

            EventManager.SaveUser(user);

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(amount + "x " + Item.Name + " has been added to your inventory."));
        }
    }
}
