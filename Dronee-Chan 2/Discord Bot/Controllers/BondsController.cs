using Dronee_Chan_2.Discord_Bot.Events;
using Dronee_Chan_2.Discord_Bot.Objects.UserObjects;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Controllers
{
    internal class BondsController
    {
        public DiscordGuild DiscordGuild { get; private set; }
        public DiscordGuild DC_Lair { get; private set; }

        public BondsController(DiscordGuild rVN, DiscordGuild dC_Lair)
        {
            DiscordGuild = rVN;
            DC_Lair = dC_Lair;

            EventManager.GetBondsEventRaised += EventManager_GetBondsEventRaised;
            EventManager.PayEventRaised += EventManager_PayEventRaised;
        }

        private async Task<bool> EventManager_PayEventRaised(int amount, ulong ID)
        {
            if (amount == -1)
                return true;

            string message = "";

            User user = await EventManager.LoadUser(ID);


            if (amount == 0)
            {
                user.LastSeen = DateTime.Now;
                return true;
            }
            
            user.FlightsAmount++;


            if (user.LastSeen == new DateTime(1977, 1, 1))
            {
                user.StreakCounter = 0;
                message = "First flight, streak set to 0.";
            }
            else
            {
                if (user.StreakCounter < 10)
                {
                    user.StreakCounter++;
                    message = "Streak increased to " + user.StreakCounter + ".";
                } else
                {
                    user.StreakCounter = 10;
                    message = "Streak kept at " + user.StreakCounter + ".";
                }

                if ((DateTime.Now - user.LastSeen).TotalDays > 10)
                {
                    int saversNeeded = (int)Math.Ceiling(((DateTime.Now - user.LastSeen).TotalDays - 10.0) / 7.0);
                    int saversInInventory = 0;

                    foreach (string item in user.Inventory)
                    {
                        if (item == "3")
                            saversInInventory++;
                    }
                    if (saversNeeded > saversInInventory)
                    {
                        message = "Streak reset to 0.\n";
                        user.StreakCounter = 0;
                    } else
                    {
                        for (int i = 0; i < saversNeeded; i++)
                        {
                            user.Inventory.Remove("3");
                        }
                        message = message[..^1] + ", used " + saversNeeded + " StreakSavers to keep it.";
                    }
                }
            }

            message = DiscordGuild.GetMemberAsync(user.DiscordUUID).Result.Username + " has recieved R$" + (amount + (int)(amount * 0.05 * user.StreakCounter)) + "\n" + message;

            user.Currency += (amount + (int)(amount * 0.05 * user.StreakCounter));

            user.LastSeen = DateTime.Now;

            EventManager.SaveUser(user);

            EventManager.Message(message);

            return true;
        }

        private Task<int> EventManager_GetBondsEventRaised(char Type)
        {
             return Task.FromResult(GetBondValues().Result[char.ToUpper(Type)]);
        }

        private async Task<Dictionary<char, int>> GetBondValues()
        {

            var messages = DC_Lair.GetChannelAsync(1242267229639282738).Result.GetMessagesAsync(limit: 500);

            var bonds = new Dictionary<char, int>();
            await foreach (var message in messages)
            {
                bonds.Add(message.Content[0], int.Parse(message.Content.Split(':')[1]));
            }
            return bonds;
        }
    }
}
