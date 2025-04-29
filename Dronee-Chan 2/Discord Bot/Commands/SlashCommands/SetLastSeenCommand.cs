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
    internal class SetLastSeenCommand : ApplicationCommandModule
    {
        [Command("SetLastSeen")]
        [RequireRolesSlash(RoleCheckMode.Any, "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task SetLastSeen(CommandContext ctx, [Option("ID", "The Discord UUID of the person to set the last seen date of")] string userIDS,
                                                           [Option("LastSeenDate", "The last seen date, format: dd-MM-yyyy")] string date)
        {
            await ctx.DeferResponseAsync();

            if (!DateTime.TryParse(date, out DateTime Lastseen))
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("The Last seen date was the wrong format, please use dd-MM-yyyy."));
                return;
            }

            ulong ID = 000;
            try
            {
                ID = ulong.Parse(userIDS);
            } catch (Exception c)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("The ID given was not the right format."));
                return;
            }

            if (!ctx.Guild.Members.ContainsKey(ID))
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("The ID given was not found in the server."));
                return;
            }

            User user = await EventManager.LoadUser(ID);

            if (user == null)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("User with ID " + userIDS + " does not exist."));
                return;
            }

            user.LastSeen = Lastseen;

            EventManager.SaveUser(user);

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Last seen of " + ctx.Guild.Members[user.DiscordUUID].Username + " has been updated to " + Lastseen));
        }
    }
}
