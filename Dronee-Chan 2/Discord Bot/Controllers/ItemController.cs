using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Controllers
{
    internal class ItemController
    {

        public DiscordGuild DiscordGuild { get; private set; }

        public ItemController(DiscordGuild discordGuild)
        {
            DiscordGuild = discordGuild;
        }
    }
}
