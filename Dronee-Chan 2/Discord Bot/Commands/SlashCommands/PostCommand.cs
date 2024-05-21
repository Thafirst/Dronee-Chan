using Dronee_Chan_2.Discord_Bot.Attributes;
using DSharpPlus.CommandsNext.Attributes;
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
        [SlashCommand("Post", "Replies with Pong")]
        [RequireRolesSlash(RoleCheckMode.Any, "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task Post(InteractionContext ctx, [Option("ChannelID", "The UUID of the Channel of the message.", true)] string SChannelID,
                                                                        [Option("Message", "The message to send.", true)] string message)
        {
            await ctx.DeferAsync(true);
            ulong channelID = 0;
            try
            {
                channelID = ulong.Parse(SChannelID);
            } catch (Exception ex)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("The channel UUID was incorrect."));
                return;
            }

            await ctx.Guild.GetChannel(channelID).SendMessageAsync(message);

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("The message has been sent!"));
        }
    }
}
