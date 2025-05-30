﻿using Dronee_Chan_2.Discord_Bot.Attributes;
using Dronee_Chan_2.Discord_Bot.Events;
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
    internal class PostContractCommand : ApplicationCommandModule
    {
        [Command("PostContract")]
        [RequireRolesSlash(RoleCheckMode.Any, "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task PostContract(CommandContext ctx, [Option("Event", "Name of the event to post.")] string Name, [Option("Unixtimestamp", "the Unix timestamp for the time for the event.")] long UnixTimestamp)
        {
            await ctx.DeferResponseAsync();

            PostContractEvent.PostContract(Name, UnixTimestamp.ToString());

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Contract has been posted."));
        }
    }
}
