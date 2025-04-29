using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Events
{
    public delegate void UserVoiceUpdatedEventHandler(VoiceStateUpdatedEventArgs args);
    class UserVoiceUpdatedEvent
    {
        // Define the event
        public static event UserVoiceUpdatedEventHandler UserVoiceUpdatedEventRaised;

        // Method to raise the event
        public static void UserVoiceUpdated(VoiceStateUpdatedEventArgs args)
        {
            // Check if there are any subscribers to the event
            if (UserVoiceUpdatedEventRaised != null)
            {
                // Raise the event
                UserVoiceUpdatedEventRaised(args);
            }
        }
    }
}
