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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dronee_Chan_2.Discord_Bot.Commands.SlashCommands
{
    internal class TestCommand : ApplicationCommandModule
    {
        [SlashCommand("Test", "Replies with Pong")]
        [RequireRolesSlash(RoleCheckMode.Any, "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task Test(InteractionContext ctx, [Option("Number1", "number")]long number, [Option("Number2", "ImagesPerFile")]long ImagesPerFile)
        {
            await ctx.DeferAsync();

            User user = await EventManager.LoadUserEvent(ctx.User.Id);

            double rank = await EventManager.CalculateRank(user);

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("rank: " + rank));


            //int fileIndex = ((int)number - 1) / (int)ImagesPerFile;
            //int imageIndexInFile = ((int)number - 1) % (int)ImagesPerFile + 1;

            //await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("fileIndex: " + fileIndex + "\nimageIndexInFile: " + imageIndexInFile));
        }
    }
}
