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
    internal class DatabaseGetUserCommand : ApplicationCommandModule
    {
        [SlashCommand("databasegetuser", "Fetches the info of a user from the database and prints it")]
        [RequireRolesSlash(RoleCheckMode.Any, "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task Databasegetuser(InteractionContext ctx, [Option("UUID", "The Discord UUID of the person", true)] string id)
        {
            await ctx.DeferAsync();
            ulong UUID;
            try
            {
                UUID = ulong.Parse(id);
            } catch (Exception ex)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"{id} is not a valid ID."));
                return;
            }

            User user = await EventManager.LoadUserEvent(UUID);

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"Loaded the user for ID {user.DiscordUUID} which has a last seen date of {user.LastSeen}"));
        }
    }
}
