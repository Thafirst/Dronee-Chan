using Dronee_Chan_2.Discord_Bot.Attributes;
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
    internal class EditMessageCommand : ApplicationCommandModule
    {
        [SlashCommand("EditMessage", "Used to edit any Dronee-Chan message.")]
        [RequireRolesSlash(RoleCheckMode.Any, "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task EditMessage(InteractionContext ctx,    [Option("ChannelID", "The UUID of the Channel of the message.", true)] string SChannelID, 
                                                                        [Option("MessageID", "The UUID of the message.", true)] string SMessageID,
                                                                        [Option("Message", "The new Message to replace the old message.", true)] string newMessage)
        {
            await ctx.DeferAsync(true);

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

            await ctx.Guild.GetChannel(channelID).GetMessageAsync(messageID).Result.ModifyAsync(x => x.Content = newMessage);

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("The message has been updated!"));
        }
    }
}
