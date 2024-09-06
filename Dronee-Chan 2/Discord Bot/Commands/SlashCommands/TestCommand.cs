using Dronee_Chan_2.Discord_Bot.Attributes;
using Dronee_Chan_2.Discord_Bot.Enums;
using Dronee_Chan_2.Discord_Bot.Events;
using Dronee_Chan_2.Discord_Bot.Objects.UserObjects;
using DSharpPlus;
using DSharpPlus.Commands;
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
        [Command("Test")]
        [RequireRolesSlash(RoleCheckMode.Any, "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task Test(CommandContext ctx, [Option("String", "string")]string str = "", [Option("Number2", "ImagesPerFile")]long ImagesPerFile = 0)
        {
            await ctx.DeferResponseAsync();

            //DiscordFollowupMessageBuilder builder = new DiscordFollowupMessageBuilder()
            //    .WithContent("test with button")
            //    .AddComponents(new DiscordButtonComponent(DiscordButtonStyle.Primary, "Thisisthevalueofthetestbutton", "testt"));

            //Console.WriteLine(builder);

            //await ctx.FollowUpAsync(builder);

            //StartOnboardingQuestEvent.StartOnboardingQuest(ulong.Parse(str));


            //PostContractEvent.PostContract("RCW Olympus Open Session", 1722095378.ToString());



            string message = "Event Controls";

            var NewEvent = new DiscordButtonComponent(DiscordButtonStyle.Primary, EventManagementEnums.New.ToString(), "New Event");

            var builder = new DiscordMessageBuilder()
                .WithContent(message)
                .AddComponents(NewEvent);

            await builder.SendAsync(ctx.Channel);



            //DiscordForumChannel channel = (DiscordForumChannel)ctx.Guild.GetChannel(1251217697379979375); //TODO: Fix to Contracts channel

            //foreach (var message in channel.Threads.ToList())
            //{
            //    if ((DateTime.Now - message.CreationTimestamp).TotalDays >= 5)
            //    {
            //        await message.DeleteAsync();
            //    }
            //}

            //User user = await EventManager.LoadUser(ctx.User.Id);

            //double rank = await EventManager.CalculateRank(user);

            //int fileIndex = ((int)number - 1) / (int)ImagesPerFile;
            //int imageIndexInFile = ((int)number - 1) % (int)ImagesPerFile + 1;

            //await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("fileIndex: " + fileIndex + "\nimageIndexInFile: " + imageIndexInFile));



            //await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Done"));
        }
    }
}
