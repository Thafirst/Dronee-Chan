﻿using Dronee_Chan_2.Discord_Bot.Attributes;
using DSharpPlus.Commands;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Commands.SlashCommands
{
    internal class PostCommand : ApplicationCommandModule
    {
        [Command("Post")]
        [RequireRolesSlash(RoleCheckMode.Any, "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task Post(CommandContext ctx, [Option("ChannelID", "The UUID of the Channel of the message.", true)] string SChannelID,
                                                                        [Option("Message", "The message to send.", true)] string message)
        {
            await ctx.DeferResponseAsync();
            ulong channelID = 0;
            try
            {
                channelID = ulong.Parse(SChannelID);
            } catch (Exception ex)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("The channel UUID was incorrect."));
                return;
            }

            await ctx.Guild.GetChannelAsync(channelID).Result.SendMessageAsync(message);

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("The message has been sent!"));
        }
    }
}
