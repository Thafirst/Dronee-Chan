﻿using Dronee_Chan_2.Discord_Bot.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Commands.SlashCommands.Template
{
    internal class TemplateCommand : ApplicationCommandModule
    {
        [SlashCommand("ping", "Replies with Pong")]
        [RequireRolesSlash(RoleCheckMode.Any, "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task Ping(InteractionContext ctx)
        {
            await ctx.DeferAsync();

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Pong"));
        }
    }
}

