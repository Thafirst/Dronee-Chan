using Dronee_Chan_2.Discord_Bot.Objects.UserObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Events
{
    public delegate void PostContractEventHandler(string contract, string unixTimeStamp);
    internal class PostContractEvent
    {
        // Define the event
        public static event PostContractEventHandler PostContractEventRaised;

        // Method to raise the event
        public static void PostContract(string contract, string unixTimeStamp)
        {
            // Check if there are any subscribers to the event
            if (PostContractEventRaised != null)
            {
                // Raise the event
                PostContractEventRaised(contract, unixTimeStamp);
            }
        }
    }
}
