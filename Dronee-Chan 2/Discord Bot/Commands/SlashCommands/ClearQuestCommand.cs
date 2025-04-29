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
    internal class ClearQuestCommand : ApplicationCommandModule
    {
        [Command("ClearQuest")]
        [RequireRolesSlash(RoleCheckMode.Any, "Member", "Staff", "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task Clean(CommandContext ctx, [Option("User ID", "The Discord UUID of the person you want to clear the quest from.", true)] string ID, 
                                                    [Option("Quest ID", "The Quest ID of the Quest you want to clear.", true)] string QuestID = "0")
        {
            await ctx.DeferResponseAsync();
            ulong UserID = 000;
            try{
                UserID = ulong.Parse(ID);
            }catch(Exception c)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("The ID given was not the right format."));
                return;
            }

            if (!ctx.Guild.Members.ContainsKey(UserID))
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("The ID given was not found in the server."));
                return;
            }

            User user = await EventManager.LoadUser(UserID);

            //If Quest ID is 0, clear the Onboarding Quest
            if (QuestID == "0")
            {
                user.OnboardingQuestMistakes = 0;
                user.OnboardingQuestChoices = new Dictionary<string, string>();
            }

            EventManager.SaveUser(user);


            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"{ctx.Guild.GetMemberAsync(user.DiscordUUID).Result.Username} had their quest mistakes and choices cleaned!"));
        }
    }
}

