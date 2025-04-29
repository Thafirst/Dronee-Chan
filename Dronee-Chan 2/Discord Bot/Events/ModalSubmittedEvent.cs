using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Events
{
    public delegate void ModalSubmittedEventHandler(ModalSubmittedEventArgs args);
    public class ModalSubmittedEvent
    {
        // Define the event
        public static event ModalSubmittedEventHandler ModalSubmittedEventRaised;

        // Method to raise the event
        public static void ModalSubmitted(ModalSubmittedEventArgs args)
        {
            // Check if there are any subscribers to the event
            if (ModalSubmittedEventRaised != null)
            {
                // Raise the event
                ModalSubmittedEventRaised(args);
            }
        }
    }
}
