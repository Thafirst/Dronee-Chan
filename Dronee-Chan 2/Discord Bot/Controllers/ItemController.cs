using Dronee_Chan_2.Discord_Bot.Events;
using Dronee_Chan_2.Discord_Bot.Objects.UserObjects;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dronee_Chan_2.Discord_Bot.Controllers
{
    internal class ItemController
    {

        public DiscordGuild DiscordGuild { get; private set; }

        public ItemController(DiscordGuild discordGuild)
        {
            DiscordGuild = discordGuild;
            EventManager.GetItemEventRaised += EventManager_GetItemEventRaised;
            EventManager.GetAllItemsEventRaised += EventManager_GetAllItemsEventRaised;
            EventManager.GetItemByIDEventRaised += EventManager_GetItemByIDEventRaised;
        }

        private async Task<List<Item>> EventManager_GetAllItemsEventRaised()
        {
            var messages = DiscordGuild.GetChannelAsync(1072677862227857498).Result.GetMessagesAsync(limit:500);

            var items = new List<Item>();
            await foreach (var message in messages)
            {
                Item item = ConvertFromMessage(message);
                if (item != null)
                {
                    items.Add(item);
                }
            }
            return items;
        }

        private async Task<Item> EventManager_GetItemEventRaised(string Name)
        {
            var messages = DiscordGuild.GetChannelAsync(1072677862227857498).Result.GetMessagesAsync(limit:500);

            await foreach (DiscordMessage message in messages)
            {
                Item item = ConvertFromMessage(message);
                if (item != null && item.Name.ToLower() == Name.ToLower())
                {
                    return item;
                }
            }
            return null;
        }

        private async Task<Item> EventManager_GetItemByIDEventRaised(int ID)
        {
            var messages = DiscordGuild.GetChannelAsync(1072677862227857498).Result.GetMessagesAsync(limit: 500);

            await foreach (DiscordMessage message in messages)
            {
                Item item = ConvertFromMessage(message);
                if (item != null && item.ID == ID)
                {
                    return item;
                }
            }
            return null;
        }

        private Item ConvertFromMessage(DiscordMessage message)
        {

            var pattern = @"ID:(\d+)\s*Name:(.+?)\s*Icon:(.+?)\s*Buy:(\d+)\s*Sell:(\d+)\s*Description:(.+)";
            var match = Regex.Match(message.Content, pattern);

            if (match.Success)
            {
                return new Item(int.Parse(match.Groups[1].Value),
                                    match.Groups[2].Value,
                                    match.Groups[3].Value,
                                    int.Parse(match.Groups[4].Value),
                                    int.Parse(match.Groups[5].Value),
                                    match.Groups[6].Value);
            }
            return null;
        }
    }
}
