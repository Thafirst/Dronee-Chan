using Dronee_Chan_2.Discord_Bot.Objects.UserObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Events
{
    // Define a delegate for the event
    public delegate void SaveUserEventHandler(User user);
    public delegate Task<User> LoadUserEventHandler(ulong id);
    public delegate Task<Item> GetItemEventHandler(string Name);
    public delegate Task<List<Item>> GetAllItemsEventHandler();

    internal class EventManager
    {
        // Define the event
        public static event SaveUserEventHandler SaveUserEventRaised;

        // Method to raise the event
        public static void SaveUserEvent(User user)
        {
            // Check if there are any subscribers to the event
            if (SaveUserEventRaised != null)
            {
                // Raise the event
                SaveUserEventRaised(user);
            }
        }

        // Define the event
        public static event LoadUserEventHandler LoadUserEventRaised;

        // Method to raise the event
        public static Task<User> LoadUserEvent(ulong id)
        {
            // Check if there are any subscribers to the event
            if (LoadUserEventRaised != null)
            {
                // Raise the event
                return LoadUserEventRaised(id);
            }
            return null;
        }

        // Define the event
        public static event GetItemEventHandler GetItemEventRaised;

        // Method to raise the event
        public static Task<Item> GetItem(string Name)
        {
            // Check if there are any subscribers to the event
            if (GetItemEventRaised != null)
            {
                // Raise the event
                return GetItemEventRaised(Name.TrimEnd(' '));
            }
            return null;
        }

        // Define the event
        public static event GetAllItemsEventHandler GetAllItemsEventRaised;

        // Method to raise the event
        public static Task<List<Item>> GetAllItems()
        {
            // Check if there are any subscribers to the event
            if (GetAllItemsEventRaised != null)
            {
                // Raise the event
                return GetAllItemsEventRaised();
            }
            return null;
        }
    }
}
