using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Objects.UserObjects
{
    public class User
    {
        public ulong DiscordUUID { get; set; }  // Discord unique user ID
        public int Currency { get; set; }   // Currency amount
        public int DonationAmount { get; set; }  // Donation amount
        public List<string> Inventory { get; set; }  // User's inventory
        public DateTime LastSeen { get; set; }   // Last seen date
        public int FlightsAmount { get; set; }   // Amount of flights
        public bool IsVIP { get; set; }          // VIP status
        public int StreakCounter { get; set; }   // Streak counter
        public List<string> Achievements { get; set; }  // User's achievements
        public List<string> OnboardingQuestChoices { get; set; }  // Onboarding quest choices


        public User() { }

        public User(string json)
        {
            User deserializedUser = JsonSerializer.Deserialize<User>(json);

            DiscordUUID = deserializedUser.DiscordUUID;
            Currency = deserializedUser.Currency;
            DonationAmount = deserializedUser.DonationAmount;
            Inventory = deserializedUser.Inventory;
            LastSeen = deserializedUser.LastSeen;
            FlightsAmount = deserializedUser.FlightsAmount;
            IsVIP = deserializedUser.IsVIP;
            StreakCounter = deserializedUser.StreakCounter;
            Achievements = deserializedUser.Achievements;
            OnboardingQuestChoices = deserializedUser.OnboardingQuestChoices;
        }
        [JsonConstructor]
        public User(ulong discordUUID)
        {
            DiscordUUID = discordUUID;
            Currency = 0;
            DonationAmount = 0;
            Inventory = new List<string>();
            LastSeen = new DateTime(1977, 1, 1);
            FlightsAmount = 0;
            IsVIP = false;
            StreakCounter = 0;
            Achievements = new List<string>();
            OnboardingQuestChoices = new List<string>();
        }
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public void UpdateLastSeen()
        {
            LastSeen = DateTime.Now;
        }

        public void AddToInventory(string item)
        {
            Inventory.Add(item);
        }

        public void AddAchievement(string achievement)
        {
            Achievements.Add(achievement);
        }

        public void AddOnboardingQuestChoice(string choice)
        {
            OnboardingQuestChoices.Add(choice);
        }


    }
}
