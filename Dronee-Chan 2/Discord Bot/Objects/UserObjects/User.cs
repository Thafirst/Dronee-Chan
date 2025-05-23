﻿using System;
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
        public Dictionary<string, string> OnboardingQuestChoices { get; set; }  // Onboarding quest choices
        public int OnboardingQuestMistakes { get; set; }  // Onboarding quest choices
        public bool Infected { get; set; }  // If the user if infected
        public string PRating { get; set; }  // The P.Rating of the pilot


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
            OnboardingQuestMistakes = deserializedUser.OnboardingQuestMistakes;
            Infected = deserializedUser.Infected;
            PRating = deserializedUser.PRating;
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
            OnboardingQuestChoices = new Dictionary<string, string>();
            OnboardingQuestMistakes = 0;
            Infected = false;
            PRating = "50,50,50,50,50,-";
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


    }
}
