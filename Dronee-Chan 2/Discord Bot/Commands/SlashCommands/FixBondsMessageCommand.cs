using Dronee_Chan_2.Discord_Bot.Attributes;
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
    internal class FixBondsMessageCommand : ApplicationCommandModule
    {
        [SlashCommand("FixBondsMessage", "Replies with Pong")]
        [RequireRolesSlash(RoleCheckMode.Any, "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task FixBondsMessage(InteractionContext ctx)
        {
            await ctx.DeferAsync();

            //TODO: make after the Auto Bonds are made.

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Pong"));
        }
    }
}
