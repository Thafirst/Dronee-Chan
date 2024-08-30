using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Events
{
    public delegate void ButtonPressedEventHandler(InteractionCreatedEventArgs args);
    public class ButtonPressedEvent
    {
        // Define the event
        public static event ButtonPressedEventHandler ButtonPressedEventRaised;

        // Method to raise the event
        public static void ButtonPressed(InteractionCreatedEventArgs args)
        {
            // Check if there are any subscribers to the event
            if (ButtonPressedEventRaised != null)
            {
                // Raise the event
                ButtonPressedEventRaised(args);
            }
        }
    }
}
