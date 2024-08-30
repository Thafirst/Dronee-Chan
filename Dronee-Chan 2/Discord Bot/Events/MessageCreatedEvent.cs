using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Events
{
    public delegate void MessageCreatedEventHandler(MessageCreatedEventArgs args);
    internal class MessageCreatedEvent
    {
        // Define the event
        public static event MessageCreatedEventHandler MessageCreatedEventRaised;

        // Method to raise the event
        public static void MessageCreated(MessageCreatedEventArgs args)
        {
            // Check if there are any subscribers to the event
            if (MessageCreatedEventRaised != null)
            {
                // Raise the event
                MessageCreatedEventRaised(args);
            }
        }
    }
}
