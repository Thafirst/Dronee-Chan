using Dronee_Chan_2.Discord_Bot.Events;
using Dronee_Chan_2.Discord_Bot.Objects.UserObjects;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Controllers
{
    class RankController
    {
        DiscordGuild discordGuild;

        public RankController(DiscordGuild discordGuild)
        {
            this.discordGuild = discordGuild;
            EventManager.CalculateRankEventRaised += EventManager_CalculateRankEventRaised;
        }

        private Task<double> EventManager_CalculateRankEventRaised(User user)
        {
            var ranks = discordGuild.GetChannelAsync(1006064392338669568).Result.GetMessageAsync(1006067090559611031).Result.Content.Split('\n');

            int accPoints = 0;
            int accRanks = 0;


            foreach (var rank in ranks)
            {
                var split = rank.Split(':');
                if (split[1] == "0")
                    continue;
                accPoints += int.Parse(split[1]);

                if (user.DonationAmount < accPoints)
                {
                    int previousRankPoints = accPoints - int.Parse(split[1]);
                    int rankPoints = user.DonationAmount - previousRankPoints;
                    double level = ((double)rankPoints) / (double.Parse(split[1]) / 10) + 1.0 + (double)accRanks;
                    //Console.WriteLine("level: " + level + " BarNo: " + (int)((level - (int)level) * 10));

                    return Task.FromResult(level);
                }
                accRanks += 10;

            }
            return Task.FromResult(101.0);
        }

    }
}
