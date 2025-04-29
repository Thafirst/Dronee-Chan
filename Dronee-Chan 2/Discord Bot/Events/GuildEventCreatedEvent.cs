using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Events
{
    public delegate void GuildEventCreatedEventHandler(ScheduledGuildEventCreatedEventArgs args);
    public class GuildEventCreatedEvent
    {
        // Define the event
        public static event GuildEventCreatedEventHandler GuildEventCreatedEventRaised;

        // Method to raise the event
        public static void GuildEventCreated(ScheduledGuildEventCreatedEventArgs args)
        {
            // Check if there are any subscribers to the event
            if (GuildEventCreatedEventRaised != null)
            {
                // Raise the event
                GuildEventCreatedEventRaised(args);
            }
        }
    }
}
