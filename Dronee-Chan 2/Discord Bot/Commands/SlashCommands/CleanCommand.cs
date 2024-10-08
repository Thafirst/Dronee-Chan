﻿using Dronee_Chan_2.Discord_Bot.Attributes;
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
    internal class CleanCommand : ApplicationCommandModule
    {
        [Command("Clean")]
        [RequireRolesSlash(RoleCheckMode.Any, "Member", "Staff", "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task Clean(CommandContext ctx)
        {
            await ctx.DeferResponseAsync();

            User user = await EventManager.LoadUser(ctx.User.Id);

            if (!user.Inventory.Contains("5"))
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("You do not have a ID Card Cleaner Token. You can buy one in the Raven Shop"));
                return;
            }

            user.Inventory.Remove("5");
            user.Infected = false;

            EventManager.SaveUser(user);


            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("You have been cured of your infection!"));
        }
    }
}
