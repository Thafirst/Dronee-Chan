using Dronee_Chan_2.Discord_Bot.Database.Firebase;
using Dronee_Chan_2.Discord_Bot.Events;
using Dronee_Chan_2.Discord_Bot.Objects.UserObjects;
using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Database
{
    internal class DatabaseManager
    {
        FirebaseManager FirebaseManager { get; set; }
        public DatabaseManager(string url, string token) 
        {
            FirebaseManager = new FirebaseManager(url, token);
            EventManager.SaveUserEventRaised += HandleSaveUserEvent;
            EventManager.LoadUserEventRaised += HandleLoadUserEvent;
        }

        private async Task<User> HandleLoadUserEvent(ulong id)
        {

            var user = await LoadUser(id);

            return user;
        }

        private void HandleSaveUserEvent(User user)
        {
            SaveUser(user);
        }

        public async void SaveUser(User user)
        {
            await FirebaseManager.UploadUserAsync(user);
        }

        public Task<User> LoadUser(ulong UUID)
        {
            var result = FirebaseManager.GetUserAsync(UUID).Result;
            if (result == null)
            {
                Console.WriteLine("User was Null, creating new.");
                User user = new User(UUID);
                EventManager.SaveUser(user);
                return Task.FromResult(user);
            }
            return Task.FromResult(result);
        }

    }
}
