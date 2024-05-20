using Dronee_Chan_2.Discord_Bot.Attributes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Commands.PrefixCommands
{
    internal class GeneralCommands : BaseCommandModule
    {
        [Command("ping")]
        [Description("Replies with Pong")]
        [RequireRoles(RoleCheckMode.Any, "Staff+")]
        [RequireSpecificGuild(GuildCheckMode.Any, 734214744818581575, 1006058186136096798)]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Pong");
        }

        [Command("response")]
        [Description("Replies to a message")]
        [RequireRoles(RoleCheckMode.Any, "Staff+")]
        public async Task Response(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();

            var message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel).ConfigureAwait(false);

            await ctx.Channel.SendMessageAsync(message.Result.Content);
        }
    }
}
