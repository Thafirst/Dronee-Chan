using Dronee_Chan_2.Discord_Bot.Attributes;
using Dronee_Chan_2.Discord_Bot.Events;
using Dronee_Chan_2.Discord_Bot.Objects.UserObjects;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Trees.Metadata;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Commands.SlashCommands
{
    internal class SortInventoryCommand : ApplicationCommandModule
    {
        [Command("SortInventory")]
        [RequireRolesSlash(RoleCheckMode.Any, "Member", "Staff", "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task SortInventory(CommandContext ctx)
        {
            await ctx.DeferResponseAsync();

            User user = await EventManager.LoadUser(ctx.User.Id);

            user.Inventory.Sort();

            EventManager.SaveUser(user);

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Inventory has been sorted."));
        }
    }
}
