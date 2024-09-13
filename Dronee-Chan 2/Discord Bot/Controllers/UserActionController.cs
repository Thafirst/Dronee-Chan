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
            var channel = await RVN.GetChannelAsync(1242799365672931431); //TODO: Change to RVN DC Lair Channel.
            
            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.DarkRed)
                .WithAuthor(args.Message.Author.Username)
                .WithTitle(args.Message.Author.Username + " has deleted their message")
                .WithThumbnail(args.Message.Author.AvatarUrl)
                .AddField("Message:", args.Message.Content)
                .WithTimestamp(DateTime.Now);

            await channel.SendMessageAsync(embedBuilder.Build());
        }

        private async void MessageEditedEvent_MessageEditedEventRaised(MessageUpdatedEventArgs args)
        {

            if (args.Author.IsBot)
                return;

            var channel = await RVN.GetChannelAsync(1242799365672931431); //TODO: Change to RVN DC Lair Channel.

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.DarkRed)
                .WithAuthor(args.Author.Username)
                .WithTitle(args.Author.Username + " has edited their message")
                .WithThumbnail(args.Author.AvatarUrl)
                .AddField("Old Message:", args.MessageBefore.Content)
                .AddField("New Message:", args.Message.Content)
                .WithTimestamp(DateTime.Now);

            await channel.SendMessageAsync(embedBuilder.Build());

        }

        private async void MemberLeftEvent_MemberLeftEventRaised(GuildMemberRemovedEventArgs args)
        {
            var channel = await RVN.GetChannelAsync(1242799365672931431); //TODO: Change to RVN DC Lair Channel.

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.DarkRed)
                .WithAuthor(args.Member.Username)
                .WithTitle(args.Member.Username + " has left Ravenspear")
                .WithThumbnail(args.Member.AvatarUrl)
                .WithTimestamp(DateTime.Now);

            await channel.SendMessageAsync(embedBuilder.Build());
        }
    }
}
