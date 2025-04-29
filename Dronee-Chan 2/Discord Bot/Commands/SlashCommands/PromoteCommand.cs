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
    internal class PromoteCommand : ApplicationCommandModule
    {
        ulong Crows = 1324804131939942471;
        ulong Ravens = 1027941538027798558;
        ulong Rooks = 1027942259305484359;


        [Command("Promote")]
        [RequireRolesSlash(RoleCheckMode.Any, "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task Promote(CommandContext ctx, [Option("ID", "DiscordUUID of the person to promote")] string SID)
        {
            await ctx.DeferResponseAsync();

            ulong ID = ulong.Parse(SID);

            DiscordMember member = await ctx.Channel.Guild.GetMemberAsync(ID);

            string wing = FindCurrentWing(member);
            PromoteRoles(member, wing);
            string message = "Congratulations [MENTION]! You have been enlisted into the [WING]! " +
                        "Check ⁠[WINGCHANNEL] for wing specific information and further progression, and report to your " +
                        "new wing lead [WINGLEAD] if you have any questions!";

            message = FormatMessage(member, message, wing);

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(message));
        }

        private string FormatMessage(DiscordMember member, string message, string wing)
        {
            switch (wing)
            {
                case "Civ":
                    return FormatCrows(member, message);

                case "Crows":
                    return FormatRavens(member, message);

                case "Ravens":
                    return FormatRooks(member, message);
            }
            return null;
        }

        private string FormatCrows(DiscordMember member, string message)
        {
            message = message.Replace("[MENTION]", member.Mention);
            message = message.Replace("[WING]", "51st Crows");
            message = message.Replace("[WINGCHANNEL]", member.Guild.GetChannelAsync(1027994017843122186).Result.Mention);
            message = message.Replace("[WINGLEAD]", member.Guild.GetMemberAsync(410957383355990048).Result.Mention);

            return message;
        }

        private string FormatRavens(DiscordMember member, string message)
        {
            message = message.Replace("[MENTION]", member.Mention);
            message = message.Replace("[WING]", "32nd Ravens");
            message = message.Replace("[WINGCHANNEL]", member.Guild.GetChannelAsync(1027994122688151573).Result.Mention);
            message = message.Replace("[WINGLEAD]", member.Guild.GetMemberAsync(410957383355990048).Result.Mention);

            return message;
        }

        private string FormatRooks(DiscordMember member, string message)
        {
            message = message.Replace("[MENTION]", member.Mention);
            message = message.Replace("[WING]", "13th");
            message = message.Replace("[WINGCHANNEL]", member.Guild.GetChannelAsync(1324804967013421196).Result.Mention);
            message = message.Replace("[WINGLEAD]", member.Guild.GetMemberAsync(137025073851793408).Result.Mention);

            return message;
        }

        private string FindCurrentWing(DiscordMember member)
        {
            foreach(DiscordRole role in member.Roles)
            {
                if (role.Id == Crows)
                    return "Crows";
                if (role.Id == Ravens)
                    return "Ravens";
                if (role.Id == Rooks)
                    return "Rooks";
            }
            return "Civ";

        }

        private void PromoteRoles(DiscordMember member, string wing)
        {
            switch (wing)
            {
                case "Civ":
                    PromoteToCrows(member);
                    break;

                case "Crows":
                    PromoteToRaven(member);
                    break;

                case "Ravens":
                    PromoteToRooks(member);
                    break;

                case "Rooks":
                    break;
            }
        }

        private async void PromoteToRooks(DiscordMember member)
        {
            await member.GrantRoleAsync(member.Guild.GetRoleAsync(Rooks).Result);
            await member.RevokeRoleAsync(member.Guild.GetRoleAsync(Ravens).Result);
        }

        private async void PromoteToRaven(DiscordMember member)
        {
            await member.GrantRoleAsync(member.Guild.GetRoleAsync(Ravens).Result);
            await member.RevokeRoleAsync(member.Guild.GetRoleAsync(Crows).Result);
        }

        private async void PromoteToCrows(DiscordMember member)
        {
            await member.GrantRoleAsync(member.Guild.GetRoleAsync(Crows).Result);
        }
    }
}
