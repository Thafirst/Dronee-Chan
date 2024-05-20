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
    internal class HelpCommand : ApplicationCommandModule
    {
        [SlashCommand("help", "Posts the full list of commands available")]
        [RequireRolesSlash(RoleCheckMode.Any, "Member", "Staff", "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task Help(InteractionContext ctx)
        {
            try
            {
                await ctx.DeferAsync();

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(MemberHelp()));
                Console.WriteLine(ctx.Member.Roles);


                if ((ctx.Member.Roles.Any(r => r.Name == "Staff")))
                    await ctx.Channel.SendMessageAsync(StaffHelp());
                if ((ctx.Member.Roles.Any(r => r.Name == "Staff+")))
                    await ctx.Channel.SendMessageAsync(StaffPlusHelp());


            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private string MemberHelp()
        {
            return "**__Members__**\n**Id [Discord UUID]** - Displays your personal ID card, or if a Discord UUID is supplied, the the ID card of that person.\n" +
                    "**Ranks** - Shows the different ranks acheveable from donating.\n" +
                    "**IsXModuleGood** - Answers the age old question of which modules are the best.\n" +
                    "**Donate <Amount>** - donates R$ to the PMC in order to increase your rank.\n" +
                    "**Catalog** - Displays the Ravenspear Shop.\n" +
                    "**BuyItem <Item ID>** - Buys the item with the supplied ID.\n" +
                    "**Infect <Discord UUID>** - Infects the person with the supplied UUID.\n" +
                    "**Clean** - Cleans your ID card if you have an ID Card Cleaner.";
        }
        private string StaffHelp()
        {
            return "**__Staff__**\nNothing here. :)";
        }
        private string StaffPlusHelp()
        {
            return "**__Staff+__**\n" +
                    "**PostContract <Contract name> <Unix Timestamp>** - Posts the contract with the given name with start time at the timestamp.\n" +
                    "**GiveItem <ID> <Discord UUID>** - Gives the item with the ID to the user with the UUID.\n" +
                    "**GiveBonds <Discord UUID> <Bonds Message>** - Gives the person the given bonds. eg. C33 for 33 Combat bonds.\n" +
                    "**RemoveItem <ID> <Discord UUID>** - Removes the item with the ID from the user with the UUID.\n" +
                    "**VIP <Discord UUID>** - Tags the user with VIP status.\n" +
                    "**FixBondsMessage <Message ID> <New Message>** - In case the bonds message breaks, use this to fix it.\n" +
                    "**FixContractMessage <Message ID> <New Message>** - In case something is missing from a contract or needs to be changed, use this to do so.\n" +
                    "**ManualEvent <Contract Message ID>** - Adds the Discord event for the given contract.\n" +
                    "**Activity** - Checks for member activity and prints anyone breaking the activity rule.\n" +
                    "**Promote <Discord UUID>** - Promotes the given user from member to raven.\n" +
                    "**Post <Channel ID>** <Message ID> [More Message IDs] - Posts messages with Dronee-Chan.\n" +
                    "**Say <Message>** - Posts a message in the same channel as Dronee-Chan.\n" +
                    "**LastSeen <Discord UUID>** - Prints the last seen date of the given user.\n" +
                    "**UpdateMessage <Channel ID> <Message ID> <New Message>** - Updates the given Dronee-Chan message with the new one.\n" +
                    "**SetDonation <Discord UUID> <New Donation Amount>** - Sets the donation amount of the given user.\n" +
                    "**SetCurrency <Discord UUID> <New Currency Amount>** - Sets the currency amount of the given user. \n" +
                    "**SetFlights <Discord UUID> <New Flights Amount>** - Sets the number of flights of the user to the given amount.\n" +
                    "**SetLastSeen <Discord UUID> <New Last Seen Time>** - Sets the last seen date of the user to the given time.\n" +
                    "**SetStreak <Discord UUID> <New Streak Amount>** - Sets the streak ammount of the given user.\n";
        }
    }
}
