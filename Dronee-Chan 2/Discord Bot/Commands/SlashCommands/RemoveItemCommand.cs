using Dronee_Chan_2.Discord_Bot.Attributes;
using Dronee_Chan_2.Discord_Bot.Events;
using Dronee_Chan_2.Discord_Bot.Objects.UserObjects;
using DSharpPlus.Commands;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Commands.SlashCommands
{
    internal class RemoveItemCommand : ApplicationCommandModule
    {
        [Command("RemoveItem")]
        [RequireRolesSlash(RoleCheckMode.Any, "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task RemoveItem(CommandContext ctx, [Option("ID", "The Discord UUID of the person to give the item to.")] string UserSID,
                                                         [Option("Item", "The ID of the item to give.")] long ItemLID)
        {
            await ctx.DeferResponseAsync();

            ulong ID = 000;
            try
            {
                ID = ulong.Parse(UserSID);
            } catch (Exception c)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("The ID given was not the right format."));
                return;
            }

            if (!ctx.Guild.Members.ContainsKey(ID))
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("The ID given was not found in the server."));
                return;
            }

            int ItemID = (int)ItemLID;
            User user = await EventManager.LoadUser(ID);

            if (user == null)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("User with ID " + UserSID + " does not exist."));
                return;
            }

            if (user.Inventory.Count <= 0)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(ctx.Guild.GetMemberAsync(user.DiscordUUID).Result.Username + " does not have any items in their inventory."));
                return;
            }

            Item item = await EventManager.GetItemByID(ItemID);

            if (!user.Inventory.Contains(item.ID.ToString()))
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("User with ID " + UserSID + " does not have " + item.Name + " in their inventory."));
                return;
            }

            if (item == null)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Item with ID " + ItemID + " does not exist."));
                return;
            }

            user.Inventory.Remove(item.ID.ToString());

            EventManager.SaveUser(user);

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Item " + item.Name + " has been removed from " + ctx.Guild.Members[user.DiscordUUID].Username + "'s inventory."));
        }
    }
}
