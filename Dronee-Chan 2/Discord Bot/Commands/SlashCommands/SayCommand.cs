using Dronee_Chan_2.Discord_Bot.Attributes;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Commands.SlashCommands
{
    internal class SayCommand : ApplicationCommandModule
    {
        [Command("Say")]
        [RequireRolesSlash(RoleCheckMode.Any, "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task Say(SlashCommandContext ctx, [Option("Message", "Message to get Dronee-Chan to say")] string message)
        {
            await ctx.DeferResponseAsync(true);

            await ctx.Channel.SendMessageAsync(message);

            await ctx.EditResponseAsync("Message has been posted.");
        }
    }
}
