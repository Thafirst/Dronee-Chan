using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Events
{
    public delegate void MemberLeftEventHandler(GuildMemberRemovedEventArgs args);
    public class MemberLeftEvent
    {
        // Define the event
        public static event MemberLeftEventHandler MemberLeftEventRaised;

        // Method to raise the event
        public static void MemberLeft(GuildMemberRemovedEventArgs args)
        {
            // Check if there are any subscribers to the event
            if (MemberLeftEventRaised != null)
            {
                // Raise the event
                MemberLeftEventRaised(args);
            }
        }
    }
}
