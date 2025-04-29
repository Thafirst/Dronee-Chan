using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Events
{
    public delegate void EditPRatingEventHandler(ulong ID);
    public delegate Task<string> GetPRatingEventHandler(ulong ID);
    public class PRatingEvents
    {
        // Define the event
        public static event EditPRatingEventHandler EditPRatingEventRaised;

        // Method to raise the event
        public static void EditPRating(ulong ID)
        {
            // Check if there are any subscribers to the event
            if (EditPRatingEventRaised != null)
            {
                // Raise the event
                EditPRatingEventRaised(ID);
            }
        }

        // Define the event
        public static event GetPRatingEventHandler GetPRatingEventRaised;

        // Method to raise the event
        public static Task<string> GetPRating(ulong ID)
        {
            // Check if there are any subscribers to the event
            if (GetPRatingEventRaised != null)
            {
                // Raise the event
                return GetPRatingEventRaised(ID);
            }
            return null;
        }
    }
}
