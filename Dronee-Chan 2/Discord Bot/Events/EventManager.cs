using Dronee_Chan_2.Discord_Bot.Objects.UserObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Events
{
    // Define a delegate for the event
    public delegate void SaveUserEventHandler(User user);
    public delegate Task<User> LoadUserEventHandler(ulong id);
    public delegate Task<Item> GetItemEventHandler(string Name);
    public delegate Task<Item> GetItemByIDEventHandler(int ID);
    public delegate Task<List<Item>> GetAllItemsEventHandler();
    public delegate Task<int> GetBondsEventHandler(char Type);
    public delegate Task<bool> PayEventHandler(int amount, ulong ID);
    public delegate void MessageEventHandler(string message);
    public delegate Task<string> GenerateIDCEventHandler(User user);
    public delegate Task<double> CalculateRankEventHandler(User user);

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
        public static event GetItemByIDEventHandler GetItemByIDEventRaised;

        // Method to raise the event
        public static Task<Item> GetItemByID(int ID)
        {
            // Check if there are any subscribers to the event
            if (GetItemByIDEventRaised != null)
            {
                // Raise the event
                return GetItemByIDEventRaised(ID);
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

        // Define the event
        public static event GetBondsEventHandler GetBondsEventRaised;

        // Method to raise the event
        public static Task<int> GetBonds(char Type)
        {
            // Check if there are any subscribers to the event
            if (GetBondsEventRaised != null)
            {
                // Raise the event
                return GetBondsEventRaised(Type);
            }
            return null;
        }

        // Define the event
        public static event PayEventHandler PayEventRaised;

        // Method to raise the event
        public static Task<bool> Pay(int amount, ulong ID)
        {
            // Check if there are any subscribers to the event
            if (PayEventRaised != null)
            {
                // Raise the event
                return PayEventRaised(amount, ID);
            }
            return null;
        }

        // Define the event
        public static event MessageEventHandler MessageEventRaised;

        // Method to raise the event
        public static void Message(string message)
        {
            // Check if there are any subscribers to the event
            if (MessageEventRaised != null)
            {
                // Raise the event
                MessageEventRaised(message);
            }
        }

        // Define the event
        public static event GenerateIDCEventHandler GenerateIDCEventRaised;

        // Method to raise the event
        public static Task<string> GenerateIDC(User user)
        {
            // Check if there are any subscribers to the event
            if (GenerateIDCEventRaised != null)
            {
                // Raise the event
                return GenerateIDCEventRaised(user);
            }
            return null;
        }

        // Define the event
        public static event CalculateRankEventHandler CalculateRankEventRaised;

        // Method to raise the event
        public static Task<double> CalculateRank(User user)
        {
            // Check if there are any subscribers to the event
            if (CalculateRankEventRaised != null)
            {
                // Raise the event
                return CalculateRankEventRaised(user);
            }
            return null;
        }
    }
}
