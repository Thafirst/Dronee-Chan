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
using System.Globalization;
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



            //await foreach(var member in ctx.Guild.GetAllMembersAsync())
            //{
            //    User user = await EventManager.LoadUser(member.Id);


            //    EventManager.SaveUser(user);
            //}


            //string message = "Hello! " + "<@" + 137025073851793408 + ">" + " Hello! " + "<@" + 410957383355990048 + ">";

            //var channel = await ctx.Channel.CreateThreadAsync("Test Thread", DiscordAutoArchiveDuration.Week,DiscordChannelType.PublicThread);

            //await channel.SendMessageAsync(message);


            //var message = await ctx.Guild.GetChannelAsync(1242799365672931431).Result.GetMessageAsync(1361808019591004391);

            //Console.WriteLine(message.Embeds[0].Fields[0].Value.Split(':')[1].Split(' ')[0]);


            //await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Done"));

            //PostContractEvent.PostContract("Ravenspear Contractor Warfare (RCW) Dynamic Campaign", "1745002800");




            //#############################
            //Clean Old Database:

            //Collect data:
            //DiscordGuild DC_Lair = await ctx.Client.GetGuildAsync(1006058186136096798);
            //DiscordGuild RVN = await ctx.Client.GetGuildAsync(734214744818581575);

            //DiscordMessage StreaksMessage = await DC_Lair.GetChannelAsync(1096490894401740801).Result
            //    .GetMessageAsync(1096491145355345990);

            //DiscordMessage FlightsMessage = await DC_Lair.GetChannelAsync(1071127927854477372).Result
            //    .GetMessageAsync(1071129192625881169);

            //DiscordMessage LastseenMessage = await DC_Lair.GetChannelAsync(1071127801828212886).Result
            //    .GetMessageAsync(1071129060819882024);

            //DiscordMessage CurrencyMessage = await DC_Lair.GetChannelAsync(1006064368993189980).Result
            //    .GetMessageAsync(1012770460976566322);

            //DiscordMessage DonationMessage = await DC_Lair.GetChannelAsync(1006064368993189980).Result
            //    .GetMessageAsync(1020416176544813076);

            //DiscordMessage InventoriesMessage = await DC_Lair.GetChannelAsync(1076203785480847472).Result
            //    .GetMessageAsync(1076203923091759277);




            //Dictionary<ulong, string> Streaks = StreaksMessage.Content.Split('\n').ToDictionary(line => ulong.Parse(line.Split(':')[0]), line => line.Split(':')[1]);

            //Dictionary<ulong, string> Flights = FlightsMessage.Content.Split('\n').ToDictionary(line => ulong.Parse(line.Split(':')[0]), line => line.Split(':')[1]);

            //Dictionary<ulong, string> Lastseen = LastseenMessage.Content.Split('\n').ToDictionary(line => ulong.Parse(line.Split(':')[0]), line => line.Split(':')[1]);

            //Dictionary<ulong, string> Currency = CurrencyMessage.Content.Split('\n').ToDictionary(line => ulong.Parse(line.Split(':')[0]), line => line.Split(':')[1]);

            //Dictionary<ulong, string> Donation = DonationMessage.Content.Split('\n').ToDictionary(line => ulong.Parse(line.Split(':')[0]), line => line.Split(':')[1]);

            //Dictionary<ulong, string> Inventories = InventoriesMessage.Content.Split('\n').Skip(1).ToDictionary(line => ulong.Parse(line.Split(':')[0]), line => line.Split(':')[1]);

            //User user;
            //ulong id = 0;
            //await foreach (var member in RVN.GetAllMembersAsync())
            //{
            //    id = member.Id;

            //    if (id == 898056364688039946 || id == 964940458910416997)
            //        continue;

            //    user = new User(id);
            //    if(Currency.ContainsKey(id))
            //        user.Currency = int.Parse(Currency[id]);
            //    if (Donation.ContainsKey(id))
            //        user.DonationAmount = int.Parse(Donation[id]);
            //    if (Streaks.ContainsKey(id))
            //        user.StreakCounter = int.Parse(Streaks[id]);
            //    if (Flights.ContainsKey(id))
            //        user.FlightsAmount = int.Parse(Flights[id]);
            //    if (Inventories.ContainsKey(id))
            //        user.Inventory = Inventories[id].Split(',').ToList();
            //    if (Lastseen.ContainsKey(id))
            //        user.LastSeen = DateTime.ParseExact(Lastseen[id].Trim(), "dd-MM-yyyy", CultureInfo.InvariantCulture);

            //    EventManager.SaveUser(user);

            //    Console.WriteLine("Saved " + member.Username + " to the new database.");
            //}




            //#############################


            //DiscordFollowupMessageBuilder builder = new DiscordFollowupMessageBuilder()
            //    .WithContent("test with button")
            //    .AddComponents(new DiscordButtonComponent(DiscordButtonStyle.Primary, "Thisisthevalueofthetestbutton", "testt"));

            //Console.WriteLine(builder);

            //await ctx.FollowUpAsync(builder);

            //if(str == "")
            //{
            //    await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("please supply an ID"));
            //    return;
            //}

            //StartOnboardingQuestEvent.StartOnboardingQuest(ulong.Parse(str));


            //PostContractEvent.PostContract("RCW Olympus Open Session", 1722095378.ToString());



            //string message = "Event controls";

            //var NewEvent = new DiscordButtonComponent(DiscordButtonStyle.Primary, EventManagementEnums.NewEventManagement.ToString(), "New Event");

            //var builder = new DiscordMessageBuilder()
            //    .WithContent(message)
            //    .AddComponents(NewEvent);

            //await builder.SendAsync(ctx.Channel);



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



            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Done"));
        }
    }
}
