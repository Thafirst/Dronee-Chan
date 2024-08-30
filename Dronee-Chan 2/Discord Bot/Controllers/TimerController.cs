using Dronee_Chan_2.Discord_Bot.Events;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Text.RegularExpressions;
using System.Timers;

namespace Dronee_Chan_2.Discord_Bot.Controllers
{
    internal class TimerController
    {
        System.Timers.Timer DailyTimer = new System.Timers.Timer();
        List<System.Timers.Timer> EventTimers = new List<System.Timers.Timer>();
        DiscordGuild RVN;

        public TimerController(DiscordGuild rVN)
        {
            RVN = rVN;

            SetupDailyTimer();
            SetupEventTimers();
        }

        private void SetupDailyTimer()
        {
            DateTime tomorrow = new DateTime(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day, 12, 0, 0);
            TimeSpan ts = tomorrow - DateTime.Now;

            DailyTimer.Interval = (ts.TotalMilliseconds);
            DailyTimer.AutoReset = false;
            DailyTimer.Elapsed += (sender, e) => DailyTimerCallback(sender, e);
            DailyTimer.Start();
        }

        private void DailyTimerCallback(object sender, ElapsedEventArgs e)
        {
            DailyCheckEvent.DailyCheck();

            SetupDailyTimer();
        }

        private void SetupEventTimers()
        {
            ContractTimers();
        }

        private void ContractTimers()
        {

            DiscordForumChannel channel = (DiscordForumChannel)RVN.GetChannelAsync(1251217697379979375).Result; //TODO: Fix to Contracts channel

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
            if (!IsFutureDate(DT))
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
                RVN.GetRoleAsync(1006063651297447996).Result.Mention); //TODO: update to Member role
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
