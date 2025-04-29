using Dronee_Chan_2.Discord_Bot.Attributes;
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
        public async Task Post(CommandContext ctx, [Option("MessageID", "The ID of the message to send.", true)] string SMessageID, 
                                                   [Option("ChannelID", "The UUID of the Channel of the message.", true)] string SChannelID)
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

            DiscordChannel channel = await ctx.Guild.GetChannelAsync(channelID);

            var split = SMessageID.Split(' ');
            var messages = split;

            for(int i = 0; i < split.Length; i++)
            {
                messages[i] = ctx.Channel.GetMessageAsync(ulong.Parse(split[i])).Result.Content;

                if (messages[i].Length > 1950)
                {
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("The message " + split[i] + " was too long at " + messages[i].Length + " characters (1950 cap)"));
                    return;
                }
            }

            for(int i = 0; i < messages.Length; i++)
            {
                await channel.SendMessageAsync(messages[i]);
            }


            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("The message(s) has been sent!"));
        }
    }
}
