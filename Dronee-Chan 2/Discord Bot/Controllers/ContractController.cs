using Dronee_Chan_2.Discord_Bot.Events;
using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Controllers
{
    internal class ContractController
    {

        DiscordGuild DC_Lair;
        DiscordGuild RVN;

        public ContractController(DiscordGuild dC_Lair, DiscordGuild rVN)
        {
            DC_Lair = dC_Lair;
            RVN = rVN;

            PostContractEvent.PostContractEventRaised += PostContractEvent_PostContractEventRaised;
            DailyCheckEvent.DailyCheckEventRaised += DailyCheckEvent_DailyCheckEventRaised;
        }

        private void DailyCheckEvent_DailyCheckEventRaised()
        {
            AutoEventHandling();
            CleanContracts();
        }

        private async void CleanContracts()
        {
            DiscordForumChannel channel = (DiscordForumChannel)RVN.GetChannelAsync(1251217697379979375).Result; //TODO: Fix to Contracts channel

            foreach (var message in channel.Threads.ToList())
            {
                if ((DateTime.Now - message.CreationTimestamp).TotalDays >= 7)
                {
                    await message.DeleteAsync();
                }
            }
        }

        private async void AutoEventHandling()
        {
            var messages = DC_Lair.GetChannelAsync(1251241109951217794).Result.GetMessagesAsync();

            await foreach (var message in messages)
            {
                if (message.Content == "Event controls")
                    continue;

                var split = message.Content.Split('\n');
                if (CheckIfInTwoDays(split[1]))
                {
                    PostContractEvent.PostContract(split[0], new DateTimeOffset(new DateTime(
                    DateTime.Now.AddDays(2).Year,
                        DateTime.Now.AddDays(2).Month,
                        DateTime.Now.AddDays(2).Day,
                        int.Parse(split[2].Split('.')[0]),
                        int.Parse(split[2].Split('.')[1]),
                        0)).ToUnixTimeSeconds().ToString());
                }
            }
        }

        private bool CheckIfInTwoDays(string day)
        {
            if (day != DateTime.Today.AddDays(2).DayOfWeek.ToString())
                return false;

            return true;
        }




        private string getContractText(string contractName)
        {
            var lines = DC_Lair.GetChannelAsync(1104199917813112882).Result.GetMessageAsync(1104200190551932928).Result.Content.Split("\n");

            foreach (var line in lines)
            {
                var split = line.Split(':');
                if (split[0] == contractName)
                    return DC_Lair.GetChannelAsync(1104199917813112882).Result.GetMessageAsync(ulong.Parse(split[1])).Result.Content;
            }
            return null;

        }

        private async void PostContractEvent_PostContractEventRaised(string contract, string unixTimeStamp)
        {

            DiscordForumChannel DFC = (DiscordForumChannel)RVN.GetChannelAsync(1251217697379979375).Result; //TODO: update to RVN contract channel

            string ContractText = getContractText(contract);

            ForumPostBuilder PostBuilder = new ForumPostBuilder()
                .WithMessage(new DiscordMessageBuilder().WithContent(ContractText.Replace("[UNIXTIME]", unixTimeStamp)))
                .WithName(contract + " - <t:" + unixTimeStamp + ":f>");

            var post = await DFC.CreateForumPostAsync(PostBuilder);

            await post.Message.ModifyEmbedSuppressionAsync(true);
        }
    }
}
