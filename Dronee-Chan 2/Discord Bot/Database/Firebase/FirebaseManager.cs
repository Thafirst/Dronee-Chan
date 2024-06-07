using Dronee_Chan_2.Discord_Bot.Objects.UserObjects;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Database.Firebase
{
    internal class FirebaseManager
    {
        private readonly FirebaseClient _firebaseClient;

        public FirebaseManager(string firebaseUrl, string firebaseSecret)
        {

            _firebaseClient = new FirebaseClient(firebaseUrl, new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(firebaseSecret)
            });
        }

        public async Task UploadUserAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            string userJson = user.ToJson();

            await _firebaseClient.Child("users").Child(user.DiscordUUID.ToString()).PutAsync(userJson);
        }

        public async Task<User> GetUserAsync(ulong discordUUID)
        {
            try
            {
                string userJson = await _firebaseClient.Child("users").Child(discordUUID.ToString()).OnceSingleAsync<string>();

                if (userJson == null)
                    return null;

                return new User(userJson);

            } catch (Exception ex)
            {
                Console.WriteLine("Error retrieving user from Firebase: " + ex.Message);
                return null;
            }
        }
    }
}
