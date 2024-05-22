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

        ulong MessageChannel = 1242799365672931431;

        public DiscordGuild DiscordGuild { get; private set; }

        public MessageController(DiscordGuild discordGuild)
        {
            DiscordGuild = discordGuild;

            EventManager.MessageEventRaised += EventManager_MessageEventRaised;
        }

        private void EventManager_MessageEventRaised(string message)
        {
            DiscordGuild.GetChannel(MessageChannel).SendMessageAsync(message);
        }
    }
}
