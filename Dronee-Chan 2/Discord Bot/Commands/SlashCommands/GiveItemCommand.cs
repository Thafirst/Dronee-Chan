using Dronee_Chan_2.Discord_Bot.Attributes;
using Dronee_Chan_2.Discord_Bot.Events;
using Dronee_Chan_2.Discord_Bot.Objects.UserObjects;
using DSharpPlus.Commands;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Commands.SlashCommands
{
    internal class GiveItemCommand : ApplicationCommandModule
    {
        [Command("GiveItem")]
        [RequireRolesSlash(RoleCheckMode.Any, "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task GiveItem(CommandContext ctx, [Option("ID","The Discord UUID of the person to give the item to.")] string SID,
                                                           [Option("Item","The ID of the item to give.")] long LID)
        {
            await ctx.DeferResponseAsync();

            ulong ID = ulong.Parse(SID);

            int ItemID = (int)LID;

            User user = await EventManager.LoadUser(ID);

            if (user == null)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("User with ID " + SID + " does not exist."));
                return;
            }

            if(user.Inventory.Count >= 15)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(ctx.Guild.GetMemberAsync(user.DiscordUUID).Result.Username + " does not have room for any items in their inventory."));
                return;
            }

            Item item = await EventManager.GetItemByID(ItemID);

            if(item == null)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Item with ID " + LID + " does not exist."));
                return;
            }

            user.Inventory.Add(item.ID.ToString());

            EventManager.SaveUser(user);

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(ctx.Guild.GetMemberAsync(user.DiscordUUID).Result.Username + " has been given 1x " + item.Name ));
        }
    }
}
