using Dronee_Chan_2.Discord_Bot.Events;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Controllers
{
    internal class UserActionController
    {
        DiscordGuild RVN;

        public UserActionController(DiscordGuild rVN)
        {
            RVN = rVN;

            MemberLeftEvent.MemberLeftEventRaised += MemberLeftEvent_MemberLeftEventRaised;
            MessageEditedEvent.MessageEditedEventRaised += MessageEditedEvent_MessageEditedEventRaised;
            MessageDeletedEvent.MessageDeletedEventRaised += MessageDeletedEvent_MessageDeletedEventRaised;
        }

        private async void MessageDeletedEvent_MessageDeletedEventRaised(MessageDeletedEventArgs args)
        {
            if (args.Message is null || args.Message.Author != null && args.Message.Author.IsBot)
                return;

            var channel = await RVN.GetChannelAsync(898075355145961502); //TODO: Change to RVN DC Lair Channel.

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.DarkRed)
                .WithTimestamp(DateTime.Now)
                .WithFooter("In: " + args.Message.Channel.Name);

            if (args.Message.Author != null)
            {
                embedBuilder.WithThumbnail(args.Message.Author.AvatarUrl)
                .AddField("Message:", args.Message.Content)
                .WithAuthor(args.Message.Author.Username)
                .WithTitle(args.Message.Author.Username + " has deleted their message");

            } else
            {
                embedBuilder
                .AddField("Message:", "No Message Found")
                .WithAuthor("Author-Not-Found")
                .WithTitle("Author-Not-Found" + " has deleted their message");
            }

                await channel.SendMessageAsync(embedBuilder.Build());
        }

        private async void MessageEditedEvent_MessageEditedEventRaised(MessageUpdatedEventArgs args)
        {

            if (args.Author.IsBot)
                return;

            string oldMessage = "The old message wasnt cached, sowwy!";

            if (args.MessageBefore is not null)
                oldMessage = args.MessageBefore.Content;

            if (oldMessage == args.Message.Content)
                return;

            var channel = await RVN.GetChannelAsync(898075355145961502); //TODO: Change to RVN DC Lair Channel.


            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.DarkRed)
                .WithAuthor(args.Author.Username)
                .WithTitle(args.Author.Username + " has edited their message")
                .WithThumbnail(args.Author.AvatarUrl)
                .WithTimestamp(DateTime.Now)
                .WithFooter("Channel: " + args.Message.Channel.Name);

            if ((args.Message.Content.Length + oldMessage.Length) >= 1000)
            {
                oldMessage = oldMessage + "\n\nNew Message:" + args.Message.Content;  
                embedBuilder.AddField("Old message(message too large):", oldMessage.Substring(0,999));
            } else
            {
                embedBuilder
                .AddField("Old Message:", oldMessage)
                .AddField("New Message:", args.Message.Content);
            }
                await channel.SendMessageAsync(embedBuilder.Build());

        }

        private async void MemberLeftEvent_MemberLeftEventRaised(GuildMemberRemovedEventArgs args)
        {
            var channel = await RVN.GetChannelAsync(898075355145961502); //TODO: Change to RVN DC Lair Channel.

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.DarkRed)
                .WithAuthor(args.Member.Username)
                .WithTitle(args.Member.Username + " has left Ravenspear")
                .WithThumbnail(args.Member.AvatarUrl)
                .WithTimestamp(DateTime.Now);

            await channel.SendMessageAsync(embedBuilder.Build());

            if((DateTime.Now - args.Member.JoinedAt).TotalMinutes < 10)
            {
                await RVN.GetChannelAsync(734214744818581578).Result.SendMessageAsync("Aaaaaaaaaaaaaand they're gone..");
            }
        }
    }
}
