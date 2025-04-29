using Dronee_Chan_2.Discord_Bot.Events;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Controllers
{
    internal class MessageController
    {

        ulong MessageChannel = 1021923123894947901; //TODO: Change to DC-Lair Channel - test-message-channel

        public DiscordGuild DiscordGuild { get; private set; }

        public MessageController(DiscordGuild discordGuild)
        {
            DiscordGuild = discordGuild;

            EventManager.MessageEventRaised += EventManager_MessageEventRaised;
        }

        private void EventManager_MessageEventRaised(string message)
        {
            DiscordGuild.GetChannelAsync(MessageChannel).Result.SendMessageAsync(message);
        }
    }
}
