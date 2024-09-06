using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Events
{
    public delegate void MemberJoinedEventHandler(GuildMemberAddedEventArgs args);
    public class MemberJoinedEvent
    {
        // Define the event
        public static event MemberJoinedEventHandler MemberJoinedEventRaised;

        // Method to raise the event
        public static void MemberJoined(GuildMemberAddedEventArgs args)
        {
            // Check if there are any subscribers to the event
            if (MemberJoinedEventRaised != null)
            {
                // Raise the event
                MemberJoinedEventRaised(args);
            }
        }
    }
}
