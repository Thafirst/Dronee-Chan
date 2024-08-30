using Dronee_Chan_2.Discord_Bot.Attributes;
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
    internal class EditMessageCommand : ApplicationCommandModule
    {
        [Command("EditMessage")]
        [RequireRolesSlash(RoleCheckMode.Any, "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task EditMessage(CommandContext ctx,    [Option("ChannelID", "The UUID of the Channel of the message.", true)] string SChannelID, 
                                                                        [Option("MessageID", "The UUID of the message.", true)] string SMessageID,
                                                                        [Option("Message", "The new Message to replace the old message.", true)] string newMessage)
        {
            await ctx.DeferResponseAsync();

            ulong channelID = 0;
            ulong messageID = 0;


            try
            {
                channelID = ulong.Parse(SChannelID);
                messageID = ulong.Parse(SMessageID);
            }catch (Exception ex)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Either the channel UUID or the message UUID was incorrect."));
                return;
            }

            await ctx.Guild.GetChannelAsync(channelID).Result.GetMessageAsync(messageID).Result.ModifyAsync(x => x.Content = newMessage);

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("The message has been updated!"));
        }
    }
}
