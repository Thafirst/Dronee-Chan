using Dronee_Chan_2.Discord_Bot.Attributes;
using Dronee_Chan_2.Discord_Bot.Events;
using Dronee_Chan_2.Discord_Bot.Objects.UserObjects;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Trees.Metadata;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Commands.SlashCommands
{
    internal class CatalogCommand : ApplicationCommandModule
    {
        [Command("Catalog")]
        [TextAlias("Shop")]
        [RequireRolesSlash(RoleCheckMode.Any, "Member", "Staff", "Staff+")]
        [RequireSpecificGuildSlash(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task Catalog(CommandContext ctx)
        {
            await ctx.DeferResponseAsync();

            List<Item> items = await EventManager.GetAllItems();

            string tokens = "";

            DiscordEmbedBuilder discordEmbedBuilder = new DiscordEmbedBuilder();
            discordEmbedBuilder.WithTitle("Raven Shop");
            discordEmbedBuilder.WithColor(new DiscordColor("#7F0000"));

            foreach (Item item  in items.Where(x => x.ID < 100).ToList())
            {
                if(item.ID == 1 || item.ID == 2)
                    continue;
                tokens += "**" + item.Name + "**\nPrice: R$" + item.BuyPrice + "\nDescription: " + item.Description + "\nID: " + item.ID + "\n\n";
            }

            discordEmbedBuilder.AddField("Tokens", tokens);

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(discordEmbedBuilder.Build()));
        }
    }
}
