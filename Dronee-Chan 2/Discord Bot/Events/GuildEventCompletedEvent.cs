using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Events
{
    public delegate void GuildEventCompletedEventHandler(ScheduledGuildEventCompletedEventArgs args);
    public class GuildEventCompletedEvent
    {
        // Define the event
        public static event GuildEventCompletedEventHandler GuildEventCompletedEventRaised;

        // Method to raise the event
        public static void GuildEventCompleted(ScheduledGuildEventCompletedEventArgs args)
        {
            // Check if there are any subscribers to the event
            if (GuildEventCompletedEventRaised != null)
            {
                // Raise the event
                GuildEventCompletedEventRaised(args);
            }
        }
    }
}
