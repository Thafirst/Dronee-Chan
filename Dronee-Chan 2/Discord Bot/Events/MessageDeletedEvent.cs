using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Events
{
    public delegate void MessageDeletedEventHandler(MessageDeletedEventArgs args);
    internal class MessageDeletedEvent
    {
        // Define the event
        public static event MessageDeletedEventHandler MessageDeletedEventRaised;

        // Method to raise the event
        public static void MessageDeleted(MessageDeletedEventArgs args)
        {
            // Check if there are any subscribers to the event
            if (MessageDeletedEventRaised != null)
            {
                // Raise the event
                MessageDeletedEventRaised(args);
            }
        }
    }
}
