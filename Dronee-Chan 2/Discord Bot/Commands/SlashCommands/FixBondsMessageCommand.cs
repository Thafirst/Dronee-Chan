using Dronee_Chan_2.Discord_Bot.Attributes;
using Dronee_Chan_2.Discord_Bot.Enums;
using DSharpPlus.Commands;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Commands.SlashCommands
{
    internal class FixBondsMessageCommand : ApplicationCommandModule
    {
        [Command("FixBondsMessage")]
        [RequireRolesSlash(RoleCheckMode.Any, "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task FixBondsMessage(CommandContext ctx, [Option("ID", "ID of the message")] string sid, [Option("Message", "The new Bonds message")] string text)
        {
            await ctx.DeferResponseAsync();

            DiscordMessage message = await ctx.Guild.GetChannelAsync(898075355145961502).Result.GetMessageAsync(ulong.Parse(sid));
            DiscordEmbed embed = message.Embeds.First();
            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder(embed);
            embedBuilder.ClearFields();
            embedBuilder.AddField("Bonds", text);

            List<DiscordButtonComponent> discordButtons = new List<DiscordButtonComponent>();
            discordButtons.Add(new DiscordButtonComponent(DiscordButtonStyle.Primary, BondsPayoutEnums.payoutBondsPayout.ToString(), "Payout"));
            discordButtons.Add(new DiscordButtonComponent(DiscordButtonStyle.Secondary, BondsPayoutEnums.EditBondsPayout.ToString(), "Edit Bonds"));

            DiscordMessageBuilder discordMessageBuilder = new DiscordMessageBuilder()
                .AddEmbed(embedBuilder.Build())
                .AddComponents(discordButtons.ToArray());

            await message.ModifyAsync(discordMessageBuilder);

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Bonds message has been fixed."));
        }

        private async void EditBondsModalHandler(DiscordMessage message, string text)
        {
            //message = args.Interaction.Message;
            //var OriginalEmbed = message.Embeds[0];
            //DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder()
            //    .WithTitle(OriginalEmbed.Title)
            //    .WithTimestamp(OriginalEmbed.Timestamp);


            //string title = "";
            //string value = "";

            //foreach (var field in message.Embeds.First().Fields.ToList())
            //{
            //    title = "";
            //    value = "";
            //    title = field.Name;
            //    if (args.Values.Keys.Contains(field.Name))
            //    {
            //        value = args.Values.FirstOrDefault(x => x.Key == field.Name).Value;
            //        Console.WriteLine("We found it " + value);
            //    } else
            //    {
            //        value = field.Value;
            //    }
            //    embedBuilder.AddField(title, value, true);
            //    Console.WriteLine("Title: " + title + " Value: " + value);
            //}

            //List<DiscordButtonComponent> discordButtons = new List<DiscordButtonComponent>();
            //discordButtons.Add(new DiscordButtonComponent(DiscordButtonStyle.Primary, BondsPayoutEnums.payoutBondsPayout.ToString(), "Payout"));
            //discordButtons.Add(new DiscordButtonComponent(DiscordButtonStyle.Secondary, BondsPayoutEnums.EditBondsPayout.ToString(), "Edit Bonds"));

            //DiscordWebhookBuilder discordMessageBuilder = new DiscordWebhookBuilder()
            //    .AddEmbed(embedBuilder.Build())
            //    .AddComponents(discordButtons.ToArray());


            //Console.WriteLine("Channel ID: " + message.ChannelId + " Message ID: " + message.Id);
            //var channel = await RVN.GetChannelAsync(message.ChannelId);

            //message = await channel.GetMessageAsync(message.Id);

            ////await channel.SendMessageAsync(discordMessageBuilder);
            //await args.Interaction.EditOriginalResponseAsync(discordMessageBuilder);
            ////await message.ModifyAsync(discordMessageBuilder);
            ////await RVN.GetChannelAsync(message.ChannelId).Result.GetMessageAsync(message.Id).Result.ModifyAsync(discordMessageBuilder);
        }
    }
}
