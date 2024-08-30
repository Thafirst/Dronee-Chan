using Dronee_Chan_2.Discord_Bot.Attributes;
using Dronee_Chan_2.Discord_Bot.Events;
using Dronee_Chan_2.Discord_Bot.Objects.UserObjects;
using DSharpPlus.Commands;
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
        [Command("Id")]
        [RequireRolesSlash(RoleCheckMode.Any, "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task Id(CommandContext ctx, [Option("ID","The Discord UUID of the person you want the ID Card from.")]string SID = "")
        {
            await ctx.DeferResponseAsync();

            if(SID == "")
                SID = ctx.User.Id.ToString();

            ulong UUID = ulong.Parse(SID);

            User user = await EventManager.LoadUser(UUID);

            string path = await EventManager.GenerateIDC(user);

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddFile(fs));
            }
        }
    }
}
