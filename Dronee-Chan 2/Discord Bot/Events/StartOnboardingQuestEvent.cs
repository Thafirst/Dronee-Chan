using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Events
{
    public delegate void StartOnboardingQuestEventHandler(ulong UUID);
    internal class StartOnboardingQuestEvent
    {
        // Define the event
        public static event StartOnboardingQuestEventHandler StartOnboardingQuestEventRaised;

        // Method to raise the event
        public static void StartOnboardingQuest(ulong UUID)
        {
            // Check if there are any subscribers to the event
            if (StartOnboardingQuestEventRaised != null)
            {
                // Raise the event
                StartOnboardingQuestEventRaised(UUID);
            }
        }
    }
}
