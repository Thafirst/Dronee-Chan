using Dronee_Chan_2.Discord_Bot.Attributes;
using Dronee_Chan_2.Discord_Bot.Events;
using Dronee_Chan_2.Discord_Bot.Objects.UserObjects;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Commands.SlashCommands
{
    internal class IdCommand : ApplicationCommandModule
    {
        [SlashCommand("Id", "Displays your personal ID card, or if a Discord UUID is supplied, the the ID card of that person.")]
        [RequireRolesSlash(RoleCheckMode.Any, "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task Id(InteractionContext ctx, [Option("ID","The Discord UUID of the person you want the ID Card from.")]string SID = "")
        {
            await ctx.DeferAsync();

            if(SID == "")
                SID = ctx.User.Id.ToString();

            ulong UUID = ulong.Parse(SID);

            User user = await EventManager.LoadUserEvent(UUID);

            string path = await EventManager.GenerateIDC(user);

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddFile(fs));
            }
        }
    }
}
