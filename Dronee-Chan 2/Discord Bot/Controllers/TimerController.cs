using Dronee_Chan_2.Discord_Bot.Events;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;
using System;
using System.Text.RegularExpressions;
using System.Timers;
using Timer = System.Threading.Timer;

namespace Dronee_Chan_2.Discord_Bot.Controllers
{
    internal class TimerController
    {
        Timer DailyTimer;
        List<System.Timers.Timer> EventTimers = new List<System.Timers.Timer>();
        DiscordGuild RVN;

        public TimerController(DiscordGuild rVN)
        {
            RVN = rVN;

            Console.WriteLine(DateTime.Today);

            SetupDailyTimer();
            SetupEventTimers();
            GuildEventCreatedEvent.GuildEventCreatedEventRaised += GuildEventCreatedEvent_GuildEventCreatedEventRaised;
            MessageCreatedEvent.MessageCreatedEventRaised += MessageCreatedEvent_MessageCreatedEventRaised;
        }

        private void GuildEventCreatedEvent_GuildEventCreatedEventRaised(DSharpPlus.EventArgs.ScheduledGuildEventCreatedEventArgs args)
        {
            SetupTimer(args.Event);
        }

        private void MessageCreatedEvent_MessageCreatedEventRaised(MessageCreatedEventArgs args)
        {
            if (args.Author.Id != 964940458910416997 && args.Author.Id != 898056364688039946 || !args.Message.Content.Contains("**CONTRACT AVAILABLE**"))
                return;
            if(args.Channel is not DiscordThreadChannel)
                return;

            SetupPreEventTimer(args.Message.Channel as DiscordThreadChannel);

        }

        private void SetupDailyTimer()
        {
            DateTime tomorrow = DateTime.Today.AddDays(1).AddHours(12);
            TimeSpan ts = tomorrow - DateTime.Now;
            //TimeSpan temp = TimeSpan.FromMinutes(1);

            DailyTimer = new Timer(DailyTimerCallback, null, (int)ts.TotalMilliseconds, (int)TimeSpan.FromDays(1).TotalMilliseconds);
        }

        private void DailyTimerCallback(object sender)
        {
            //Console.WriteLine("Daily Call");

            DailyCheckEvent.DailyCheck();
        }

        private void SetupEventTimers()
        {
            ContractTimers();
        }

        private void ContractTimers()
        {

            DiscordForumChannel channel = (DiscordForumChannel)RVN.GetChannelAsync(1019938900036305029).Result; //TODO: Fix to Contracts channel

            foreach (var message in channel.Threads.ToList())
            {
                SetupPreEventTimer(message);
            }

            foreach (var Event in RVN.GetEventsAsync().Result)
            {
                SetupTimer(Event);
            }
        }

        private void SetupPreEventTimer(DiscordThreadChannel thread)
        {
            var split = Regex.Split(Regex.Split(thread.Name, "t:")[1], ":f")[0];
            PreEventTimer(UnixTimeStampToDateTime(double.Parse(split)), thread);
        }

        private void PreEventTimer(DateTime DT, DiscordThreadChannel Channel)
        {
            if (!IsFutureDate(DT.ToLocalTime().AddHours(-1)))
                return;

            System.Timers.Timer PingTimer = new System.Timers.Timer();
            TimeSpan ts = DT.ToLocalTime().AddHours(-1) - DateTime.Now;
            PingTimer.Interval = (ts.TotalMilliseconds);
            PingTimer.Elapsed += (sender, e) => PingTimerCallback(sender, e, Channel);
            PingTimer.AutoReset = false;
            PingTimer.Start();
            EventTimers.Add(PingTimer);

        }

        private void SetupTimer(DiscordScheduledGuildEvent _event)
        {
            if (!IsFutureDate(_event.StartTime.DateTime))
                return;

            System.Timers.Timer tempTimer = new System.Timers.Timer();
            TimeSpan ts = _event.StartTime.DateTime.ToLocalTime() - DateTime.Now;
            tempTimer.Interval = (ts.TotalMilliseconds);
            tempTimer.Elapsed += (sender, e) => EventTimerCallback(sender, e, _event);
            tempTimer.AutoReset = false;
            tempTimer.Start();

            EventTimers.Add(tempTimer);
        }
        private void PingTimerCallback(object sender, ElapsedEventArgs e, DiscordThreadChannel Channel)
        {
            Channel.SendMessageAsync("This contract will commence in 1 hour! See you there!\n" +
                RVN.GetRoleAsync(822286304657932289).Result.Mention); //TODO: update to Member role
        }
        private void EventTimerCallback(object sender, ElapsedEventArgs e, DiscordScheduledGuildEvent _event)
        {
            _event.Guild.StartEventAsync(_event);
        }

        public static bool IsFutureDate(DateTime inputDateTime)
        {
            if (inputDateTime > DateTime.Now)
                return true;
            return false;
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
