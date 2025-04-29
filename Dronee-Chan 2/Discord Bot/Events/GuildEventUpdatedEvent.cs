using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Events
{
    public delegate void GuildEventUpdatedEventHandler(ScheduledGuildEventUpdatedEventArgs args);
    public class GuildEventUpdatedEvent
    {
        // Define the event
        public static event GuildEventUpdatedEventHandler GuildEventUpdatedEventRaised;

        // Method to raise the event
        public static void GuildEventUpdated(ScheduledGuildEventUpdatedEventArgs args)
        {
            // Check if there are any subscribers to the event
            if (GuildEventUpdatedEventRaised != null)
            {
                // Raise the event
                GuildEventUpdatedEventRaised(args);
            }
        }
    }
}
