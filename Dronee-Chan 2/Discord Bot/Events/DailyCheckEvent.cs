using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Events
{
    public delegate void DailyCheckEventHandler();
    internal class DailyCheckEvent
    {
        // Define the event
        public static event DailyCheckEventHandler DailyCheckEventRaised;

        // Method to raise the event
        public static void DailyCheck()
        {
            // Check if there are any subscribers to the event
            if (DailyCheckEventRaised != null)
            {
                // Raise the event
                DailyCheckEventRaised();
            }
        }
    }
}
