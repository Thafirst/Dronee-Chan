using Dronee_Chan_2.Discord_Bot.Events;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Controllers
{
    internal class EventController
    {
        DiscordGuild RVN;

        public EventController(DiscordGuild rVN)
        {
            RVN = rVN;

            MessageCreatedEvent.MessageCreatedEventRaised += MessageCreatedEvent_MessageCreatedEventRaised;
        }

        private void MessageCreatedEvent_MessageCreatedEventRaised(MessageCreatedEventArgs args)
        {
            if (args.Author.Id != 964940458910416997 && args.Author.Id != 898056364688039946 || !args.Message.Content.Contains("**CONTRACT AVAILABLE**"))
                return;

            MakeContractEvent(args.Message);
        }
        private async void MakeContractEvent(DiscordMessage message)
        {
            string content = message.Content;

            string timestamp = Regex.Split(content, @"\*\*START TIME\/DURATION:\*\*?.*(<t:?.*>)")[1].Split(':')[1];
            string name = Regex.Split(content, @"\*\*NAME:\*\*( ?.*)")[1];
            string description = "For more information on this event, check the #contracts channel." + "\n" + message.JumpLink;

            DateTime time = UnixTimeStampToDateTime(double.Parse(timestamp));

            await RVN.CreateEventAsync(name, description, 856872854234726440, DiscordScheduledGuildEventType.VoiceChannel, DiscordScheduledGuildEventPrivacyLevel.GuildOnly, time, null);

        }
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
}
