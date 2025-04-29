using Dronee_Chan_2.Discord_Bot.Enums;
using Dronee_Chan_2.Discord_Bot.Events;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Controllers
{
    internal class GuildEventController
    {
        DiscordGuild RVN;

        Dictionary<ulong, DateTime> UserJoinTimes = new Dictionary<ulong, DateTime>();
        Dictionary<ulong, int> attendance = new Dictionary<ulong, int>();
        bool TrackAttendance = false;

        public GuildEventController(DiscordGuild rVN)
        {
            RVN = rVN;

            GuildEventUpdatedEvent.GuildEventUpdatedEventRaised += GuildEventUpdatedEvent_GuildEventUpdatedEventRaised;
            GuildEventCompletedEvent.GuildEventCompletedEventRaised += GuildEventCompletedEvent_GuildEventCompletedEventRaised;
            UserVoiceUpdatedEvent.UserVoiceUpdatedEventRaised += UserVoiceUpdatedEvent_UserVoiceUpdatedEventRaised;
            ButtonPressedEvent.ButtonPressedEventRaised += ButtonPressedEvent_ButtonPressedEventRaised;
            ModalSubmittedEvent.ModalSubmittedEventRaised += ModalSubmittedEvent_ModalSubmittedEventRaised;
        }

        private void ModalSubmittedEvent_ModalSubmittedEventRaised(ModalSubmittedEventArgs args)
        {
            if (args.Interaction.Data.CustomId != BondsPayoutEnums.EditBondsPayout.ToString())
                return;

            EditBondsModalHandler(args);
        }

        private void ButtonPressedEvent_ButtonPressedEventRaised(InteractionCreatedEventArgs args)
        {
            if (!args.Interaction.Data.CustomId.Contains("BondsPayout"))
                return;

            var buttonID = args.Interaction.Data.CustomId;

            switch (Enum.Parse(typeof(BondsPayoutEnums), buttonID))
            {
                case BondsPayoutEnums.NoneBondsPayout:
                    break;
                case BondsPayoutEnums.payoutBondsPayout:
                    PayoutBonds(args.Interaction.Message);
                    break;
                case BondsPayoutEnums.EditBondsPayout:
                    EditBonds(args);
                    break;
            }
        }

        private void EditBonds(InteractionCreatedEventArgs args)
        {

            var message = args.Interaction.Message;

            var fields = message.Embeds[0].Fields;

            var modal = new DiscordInteractionResponseBuilder()
                .WithTitle("Edit Bonds - " + message.Id)
                .WithCustomId(BondsPayoutEnums.EditBondsPayout.ToString())
                .WithContent("#" + message.Id);

            foreach (var field in fields)
            {
                modal.AddComponents(new DiscordTextInputComponent("Edit " + field.Name, field.Name, value: field.Value, style: DiscordTextInputStyle.Paragraph));
            }

            args.Interaction.CreateResponseAsync(DiscordInteractionResponseType.Modal, modal);
        }

        private async void EditBondsModalHandler(ModalSubmittedEventArgs args)
        {
            var message = args.Interaction.Message;
            var OriginalEmbed = message.Embeds[0];
            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder()
                .WithTitle(OriginalEmbed.Title)
                .WithTimestamp(OriginalEmbed.Timestamp);
            

            string title = "";
            string value = "";

            foreach (var field in message.Embeds.First().Fields.ToList())
            {
                title = "";
                value = "";
                title = field.Name;
                if (args.Values.Keys.Contains(field.Name))
                {
                    value = args.Values.FirstOrDefault(x => x.Key == field.Name).Value;
                    Console.WriteLine("We found it " + value);
                } 
                else
                {
                    value = field.Value;
                }
                embedBuilder.AddField(title, value, true);
                Console.WriteLine("Title: " + title + " Value: " + value);
            }

            List<DiscordButtonComponent> discordButtons = new List<DiscordButtonComponent>();
            discordButtons.Add(new DiscordButtonComponent(DiscordButtonStyle.Primary, BondsPayoutEnums.payoutBondsPayout.ToString(), "Payout"));
            discordButtons.Add(new DiscordButtonComponent(DiscordButtonStyle.Secondary, BondsPayoutEnums.EditBondsPayout.ToString(), "Edit Bonds"));

            DiscordWebhookBuilder discordMessageBuilder = new DiscordWebhookBuilder()
                .AddEmbed(embedBuilder.Build())
                .AddComponents(discordButtons.ToArray());


            Console.WriteLine("Channel ID: " + message.ChannelId + " Message ID: " + message.Id);
            var channel = await RVN.GetChannelAsync(message.ChannelId);

            message = await channel.GetMessageAsync(message.Id);

            //await channel.SendMessageAsync(discordMessageBuilder);
            await args.Interaction.EditOriginalResponseAsync(discordMessageBuilder);
            //await message.ModifyAsync(discordMessageBuilder);
            //await RVN.GetChannelAsync(message.ChannelId).Result.GetMessageAsync(message.Id).Result.ModifyAsync(discordMessageBuilder);
        }

        private async void PayoutBonds(DiscordMessage message)
        {

            int BondsValue = await EventManager.GetBonds('C');

            var fields = message.Embeds[0].Fields;

            ulong UUID = 0;

            foreach(var field in fields)
            {
                foreach(var split in field.Value.Split('\n'))
                {
                    UUID = await UsernameToUUID(split.Split(':')[0]);
                    if (UUID == 0)
                        continue;
                    await EventManager.Pay(int.Parse(split.Split(':')[1].Split(' ')[0]) * BondsValue, UUID);
                }
            }
            await message.RespondAsync("Payout is complete");
        }

        private async Task<ulong> UsernameToUUID(string username)
        {
            await foreach(var member in RVN.GetAllMembersAsync())
            {
                if(member.Username == username)
                    return member.Id;
            }

            return 0;
        }

        private void UserVoiceUpdatedEvent_UserVoiceUpdatedEventRaised(VoiceStateUpdatedEventArgs args)
        {
            if (!TrackAttendance)
                return;
            if (args.Before is null)
                UserJoinedVoice(args.After.Member);
            if (args.After.Channel is null)
                UserLeftVoice(args.After.Member);
        }

        private void GuildEventCompletedEvent_GuildEventCompletedEventRaised(ScheduledGuildEventCompletedEventArgs args)
        {
            if (args.Event.Description.ToLower().Contains("#weekly"))
                RescheduleNextWeek(args.Event);
            if (args.Event.Description.ToLower().Contains("#notrack"))
                return;

            EventEnded(args);
        }

        private void GuildEventUpdatedEvent_GuildEventUpdatedEventRaised(ScheduledGuildEventUpdatedEventArgs args)
        {
            if (args.EventAfter.Description.ToLower().Contains("#notrack"))
                return;
            if (args.EventAfter.Status == DiscordScheduledGuildEventStatus.Active)
                EventStarted();
        }

        private void EventStarted()
        {
            TrackAttendance = true;

            var channels = RVN.GetChannelsAsync().Result.Where(x => x.Type == DiscordChannelType.Voice);

            foreach (var channel in channels)
            {
                if (channel.Users.Count == 0)
                    continue;

                foreach (var user in channel.Users)
                {
                    UserJoinTimes.Add(user.Id, DateTime.Now);
                }
            }
        }

        private async void EventEnded(ScheduledGuildEventCompletedEventArgs args)
        {
            TrackAttendance = false;

            var channels = RVN.GetChannelsAsync().Result.Where(x => x.Type == DiscordChannelType.Voice);

            foreach (var channel in channels)
            {
                if (channel.Users.Count == 0)
                    continue;

                foreach (var user in channel.Users)
                {
                    UserLeftVoice(user);
                }
            }
            List<DiscordButtonComponent> discordButtons = new List<DiscordButtonComponent>();
            DiscordMessageBuilder message = new DiscordMessageBuilder();
            discordButtons.Add(new DiscordButtonComponent(DiscordButtonStyle.Primary, BondsPayoutEnums.payoutBondsPayout.ToString(), "Payout"));
            discordButtons.Add(new DiscordButtonComponent(DiscordButtonStyle.Secondary, BondsPayoutEnums.EditBondsPayout.ToString(), "Edit Bonds"));

            DiscordEmbedBuilder discordEmbedBuilder = new DiscordEmbedBuilder()
                .WithTitle("Event Attendance for " + args.Event.Name)
                .WithTimestamp(DateTime.Now);
            int amount = attendance.Keys.Count;
            List<ulong> attendees = attendance.Keys.ToList();

            string text = "";

            foreach (var attender in attendance.Keys)
            {
                string name = RVN.GetMemberAsync(attender).Result.Username;
                string bonds = ((int)Math.Ceiling(attendance[attender] / 60.0 / 6.0)).ToString();
                if (int.Parse(bonds) > 40)
                    bonds = "40";

                text = text + name + ":" + bonds + "\n";
            }

            discordEmbedBuilder.AddField("Bonds", text);


            message.AddEmbed(discordEmbedBuilder.Build());
            message.AddComponents(discordButtons.ToArray());

            var DCchannel = await RVN.GetChannelAsync(898075355145961502); //TODO: Change to DC Lair channel.

            await DCchannel.SendMessageAsync(message);

            CreateDebriefPost(attendees, args.Event.Name + " - " + args.Event.StartTime.ToString("dd.MM.yyyy"));
        }

        private async void CreateDebriefPost(List<ulong> attendees, string EventName)
        {
            string DebriefMessage = "Thanks for flying at the latest contract! You can use this thread to discuss " +
                "how things went, follow up on your performance and ask questions or request feedback on your flying. " +
                "A staff member should post a full Tacview for you to look at soon, so look out for that (or pester them " +
                "for it if it still isn't here after an hour or two).\n";

            foreach (var attender in attendees)
            {
                DebriefMessage += "<@" + attender + ">";
            }

            DiscordChannel DFC = await RVN.GetChannelAsync(1139618100657000508); //TODO: update to RVN Debrief channel
            var thread = await DFC.CreateThreadAsync(EventName, DiscordAutoArchiveDuration.Week,DiscordChannelType.PublicThread);
            await thread.SendMessageAsync(DebriefMessage);

            //ForumPostBuilder forumPostBuilder = new ForumPostBuilder()
            //    .WithName(EventName)
            //    .WithMessage(new DiscordMessageBuilder().WithContent(DebriefMessage));
            //await DFC.CreateForumPostAsync(forumPostBuilder);
        }

        private void UserJoinedVoice(DiscordMember user)
        {
            if (UserJoinTimes.ContainsKey(user.Id))
                return;
            UserJoinTimes.Add(user.Id, DateTime.Now);
        }

        private void UserLeftVoice(DiscordMember user)
        {
            if (!UserJoinTimes.ContainsKey(user.Id))
                return;
            TimeSpan ts = DateTime.Now - UserJoinTimes[user.Id];

            if (!attendance.ContainsKey(user.Id))
                attendance.Add(user.Id, 0);
            attendance[user.Id] = attendance[user.Id] + (int)ts.TotalSeconds;
            UserJoinTimes.Remove(user.Id);
        }

        private void RescheduleNextWeek(DiscordScheduledGuildEvent _event)
        {
            RVN.CreateEventAsync(_event.Name, _event.Description, _event.ChannelId, _event.Type, _event.PrivacyLevel, _event.StartTime.AddDays(7), _event.EndTime);
        }
    }
}
