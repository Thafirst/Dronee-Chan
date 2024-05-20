using Dronee_Chan_2.Discord_Bot.Commands.SlashCommands;
using Dronee_Chan_2.Discord_Bot.Database;
using Dronee_Chan_2.Discord_Bot.Events;
using Dronee_Chan_2.Discord_Bot.Objects.UserObjects;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot
{
    class Bot
    {

        public DiscordClient Client { get; private set; }

        public InteractivityExtension Interactivity { get; private set; }

        public CommandsNextExtension Commands {  get; private set; }

        DatabaseManager databaseManager { get; set; }


        public Bot()
        {
            var json = string.Empty;
            using (var fs = File.OpenRead("Config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false))) { json = sr.ReadToEndAsync().Result; }

            var ConfigJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            databaseManager = new DatabaseManager(ConfigJson.FireURL, ConfigJson.FireToken);

            var config = new DiscordConfiguration
            {
                Token = ConfigJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug,
                Intents = DiscordIntents.All
            };

            var CommandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = [ConfigJson.Prefix],
                EnableDms = false,
            };

            var InteractivityConfiguration = new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(5),
            };

            Client = new DiscordClient(config);

            Commands = Client.UseCommandsNext(CommandsConfig);

            Client.UseInteractivity(InteractivityConfiguration);

            var SlashCommandsConfig = Client.UseSlashCommands();

            Client.SessionCreated += Client_SessionCreated;

            Commands.RegisterCommands<Commands.PrefixCommands.GeneralCommands>();

            SlashCommandsConfig.RegisterCommands<ActivityCommand>(1006058186136096798);
            SlashCommandsConfig.RegisterCommands<BuyItemCommand>(1006058186136096798);
            SlashCommandsConfig.RegisterCommands<CatalogCommand>(1006058186136096798);
            SlashCommandsConfig.RegisterCommands<CleanCommand>(1006058186136096798);
            SlashCommandsConfig.RegisterCommands<DatabaseGetUserCommand>(1006058186136096798);
            SlashCommandsConfig.RegisterCommands<DonateCommand>(1006058186136096798);
            SlashCommandsConfig.RegisterCommands<FixBondsMessageCommand>(1006058186136096798);
            SlashCommandsConfig.RegisterCommands<FixContractMessageCommand>(1006058186136096798);
            SlashCommandsConfig.RegisterCommands<GiveBondsCommand>(1006058186136096798);
            SlashCommandsConfig.RegisterCommands<GiveItemCommand>(1006058186136096798);
            SlashCommandsConfig.RegisterCommands<HelpCommand>(1006058186136096798);
            SlashCommandsConfig.RegisterCommands<IdCommand>(1006058186136096798);
            SlashCommandsConfig.RegisterCommands<InfectCommand>(1006058186136096798);
            SlashCommandsConfig.RegisterCommands<IsXModuleGoodCommand>(1006058186136096798);
            SlashCommandsConfig.RegisterCommands<LastSeenCommand>(1006058186136096798);
            SlashCommandsConfig.RegisterCommands<ManualEventCommand>(1006058186136096798);
            SlashCommandsConfig.RegisterCommands<PingCommand>(1006058186136096798);
            SlashCommandsConfig.RegisterCommands<PostCommand>(1006058186136096798);
            SlashCommandsConfig.RegisterCommands<PostContractCommand>(1006058186136096798);
            SlashCommandsConfig.RegisterCommands<PromoteCommand>(1006058186136096798);
            SlashCommandsConfig.RegisterCommands<RanksCommand>(1006058186136096798);
            SlashCommandsConfig.RegisterCommands<RemoveItemCommand>(1006058186136096798);
            SlashCommandsConfig.RegisterCommands<SetCurrencyCommand>(1006058186136096798);
            SlashCommandsConfig.RegisterCommands<SetDonateCommand>(1006058186136096798);
            SlashCommandsConfig.RegisterCommands<SetFlightsCommand>(1006058186136096798);
            SlashCommandsConfig.RegisterCommands<SetLastSeenCommand>(1006058186136096798);
            SlashCommandsConfig.RegisterCommands<SetStreakCommand>(1006058186136096798);
            SlashCommandsConfig.RegisterCommands<TestCommand>(1006058186136096798);
            SlashCommandsConfig.RegisterCommands<UpdateMessageCommand>(1006058186136096798);
            SlashCommandsConfig.RegisterCommands<VipCommand>(1006058186136096798);


        }

        public async Task RunAsync()
        {
            await Client.ConnectAsync();

            await Task.Delay(-1);
        }

        private Task Client_SessionCreated(DiscordClient sender, SessionReadyEventArgs args)
        {
            return Task.CompletedTask;
        }
    }
}
