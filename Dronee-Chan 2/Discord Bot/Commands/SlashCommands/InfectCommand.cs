using Dronee_Chan_2.Discord_Bot.Attributes;
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
    internal class InfectCommand : ApplicationCommandModule
    {
        [Command("Infect")]
        [RequireRolesSlash(RoleCheckMode.Any, "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task Infect(CommandContext ctx, [Option("ID", "The Discord UUID of the person to infect.")] string SID)
        {
            await ctx.DeferResponseAsync();

            ulong id = ulong.Parse(SID);

            DiscordMember member = await ctx.Channel.Guild.GetMemberAsync(id);

            if (member.Presence.Activities[0].Name != "War Thunder")
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(member.Username + " cannot be confirmed infected."));
                return;
            }

            DiscordRole infectedRole = await ctx.Channel.Guild.GetRoleAsync(1099018430457319565);
            await member.GrantRoleAsync(infectedRole);

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(member.Username + " has been confirmed infected, and has been tagged as such."));
        }
    }
}
