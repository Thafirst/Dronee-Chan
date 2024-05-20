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
    internal class ActivityCommand : ApplicationCommandModule
    {
        [SlashCommand("Activity", "Checks for member activity and prints anyone breaking the activity rule")]
        [RequireRolesSlash(RoleCheckMode.Any, "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task Activity(InteractionContext ctx)
        {
            await ctx.DeferAsync();

            DateTime now = DateTime.Now;
            TimeSpan span;
            int baseTime = 30;
            int time = 0;
            string message = "";
            foreach (DiscordMember member in ctx.Guild.Members.Values)
            {
                if (member.Roles.Any(r => r.Name == "Inactive") || member.Roles.Any(r => r.Name == "Denied") || member.IsBot)
                    continue;

                User user = await EventManager.LoadUserEvent(member.Id);
                time = baseTime;
                if (user.LastSeen != new DateTime(1977, 1, 1))
                {
                    time = time * 3;
                    span = now - user.LastSeen;
                } else
                {
                    span = now - member.JoinedAt.DateTime;
                }
                if (member.Roles.Any(r => r.Id == 889952510620606495))
                    time = 180;
                if (span.TotalDays >= time)
                {
                    message += member.Username + " has not been seen for " + time + " days.";
                }


            }

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(message));
        }
    }
}
