using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Events
{
    public delegate void MessageEditedEventHandler(MessageUpdatedEventArgs member);
    public class MessageEditedEvent
    {
        // Define the event
        public static event MessageEditedEventHandler MessageEditedEventRaised;

        // Method to raise the event
        public static void MessageEdited(MessageUpdatedEventArgs member)
        {
            // Check if there are any subscribers to the event
            if (MessageEditedEventRaised != null)
            {
                // Raise the event
                MessageEditedEventRaised(member);
            }
        }
    }
}
