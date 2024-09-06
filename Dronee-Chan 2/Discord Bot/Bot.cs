using Dronee_Chan_2.Discord_Bot.Commands.SlashCommands;
using Dronee_Chan_2.Discord_Bot.Controllers;
using Dronee_Chan_2.Discord_Bot.Database;
using Dronee_Chan_2.Discord_Bot.Events;
using Dronee_Chan_2.Discord_Bot.Objects.UserObjects;
using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.TextCommands;
using DSharpPlus.Commands.Processors.TextCommands.Parsing;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.DependencyInjection;
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

        ItemController IC;
        BondsController BC;
        MessageController MC;
        IDController IDC;
        RankController RC;
        ContractController CC;
        TimerController TC;
        EventController EC;
        OnboardingQuestController OQC;
        WeeklyEventManagerController WEMC;


        public Bot()
        {
            var json = string.Empty;
            using (var fs = File.OpenRead("Config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false))) { json = sr.ReadToEndAsync().Result; }

            var ConfigJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            databaseManager = new DatabaseManager(ConfigJson.FireURL, ConfigJson.FireToken);

            //var config = new DiscordConfiguration
            //{
            //    Token = ConfigJson.Token,
            //    TokenType = TokenType.Bot,
            //    AutoReconnect = true,
            //    MinimumLogLevel = LogLevel.Debug,
            //    Intents = DiscordIntents.All
            //};

            //var CommandsConfig = new CommandsNextConfiguration
            //{
            //    StringPrefixes = [ConfigJson.Prefix],
            //    EnableDms = false,
            //};

            //var InteractivityConfiguration = new InteractivityConfiguration
            //{
            //    Timeout = TimeSpan.FromMinutes(5),
            //    ResponseBehavior = InteractionResponseBehavior.Ack
            //};

            DiscordClientBuilder CBuilder = DiscordClientBuilder.CreateDefault(ConfigJson.Token, TextCommandProcessor.RequiredIntents | SlashCommandProcessor.RequiredIntents | DiscordIntents.All)
                .SetLogLevel(LogLevel.Debug);

            CBuilder.ConfigureEventHandlers
            (
                b => b
                .HandleMessageCreated(Client_MessageCreated)
                .HandleComponentInteractionCreated(Client_InteractionCreated)
                .HandleGuildMemberAdded(Client_GuildMemberJoined)
            );


            CBuilder.UseCommands(

                // we register our commands here
                extension =>
                {
                    extension.AddCommands([typeof(ActivityCommand), typeof(BuyItemCommand),
                                           typeof(CatalogCommand), typeof(CleanCommand),
                                           typeof(DatabaseGetUserCommand), typeof(DonateCommand),
                                           typeof(EditMessageCommand), typeof(FixBondsMessageCommand),
                                           typeof(GiveBondsCommand), typeof(GiveItemCommand),
                                           typeof(HelpCommand), typeof(IdCommand),
                                           typeof(InfectCommand), typeof(IsXModuleGoodCommand),
                                           typeof(LastSeenCommand), typeof(ManualEventCommand),
                                           typeof(PingCommand), typeof(PostCommand),
                                           typeof(PostContractCommand), typeof(PromoteCommand),
                                           typeof(RanksCommand), typeof(RemoveItemCommand),
                                           typeof(SetCurrencyCommand), typeof(SetDonateCommand),
                                           typeof(SetFlightsCommand), typeof(SetLastSeenCommand),
                                           typeof(SetStreakCommand), typeof(TestCommand),
                                           typeof(UpdateMessageCommand), typeof(VipCommand)
                    ]);
                    TextCommandProcessor textCommandProcessor = new(new()
                    {
                        // The default behavior is that the bot reacts to direct mentions
                        // and to the "!" prefix.
                        // If you want to change it, you first set if the bot should react to mentions
                        // and then you can provide as many prefixes as you want.
                        PrefixResolver = new DefaultPrefixResolver(true, "!").ResolvePrefixAsync
                    });

                    // Add text commands with a custom prefix (?ping)
                    extension.AddProcessors(textCommandProcessor);
                },
                new CommandsConfiguration()
                {
                    DebugGuildId = 1006058186136096798,
                    // The default value, however it's shown here for clarity
                    RegisterDefaultCommandProcessors = true
                }
            );


            Client = CBuilder.Build();

            //Client.UseInteractivity(InteractivityConfiguration);

            //Client.SessionCreated += Client_SessionCreated;

            //Client.ModalSubmitted += Client_ModalSubmitted;
            

            //Commands.RegisterCommands<Commands.PrefixCommands.GeneralCommands>();

            ////Client.MessageCreated += Client_MessageCreated;
            ////Client.InteractionCreated += Client_InteractionCreated;

            //SlashCommandsConfig.RegisterCommands<ActivityCommand>(1006058186136096798);
            //SlashCommandsConfig.RegisterCommands<BuyItemCommand>(1006058186136096798);
            //SlashCommandsConfig.RegisterCommands<CatalogCommand>(1006058186136096798);
            //SlashCommandsConfig.RegisterCommands<CleanCommand>(1006058186136096798);
            //SlashCommandsConfig.RegisterCommands<DatabaseGetUserCommand>(1006058186136096798);
            //SlashCommandsConfig.RegisterCommands<DonateCommand>(1006058186136096798);
            //SlashCommandsConfig.RegisterCommands<FixBondsMessageCommand>(1006058186136096798);
            //SlashCommandsConfig.RegisterCommands<EditMessageCommand>(1006058186136096798);
            //SlashCommandsConfig.RegisterCommands<GiveBondsCommand>(1006058186136096798);
            //SlashCommandsConfig.RegisterCommands<GiveItemCommand>(1006058186136096798);
            //SlashCommandsConfig.RegisterCommands<HelpCommand>(1006058186136096798);
            //SlashCommandsConfig.RegisterCommands<IdCommand>(1006058186136096798);
            //SlashCommandsConfig.RegisterCommands<InfectCommand>(1006058186136096798);
            //SlashCommandsConfig.RegisterCommands<IsXModuleGoodCommand>(1006058186136096798);
            //SlashCommandsConfig.RegisterCommands<LastSeenCommand>(1006058186136096798);
            //SlashCommandsConfig.RegisterCommands<ManualEventCommand>(1006058186136096798);
            //SlashCommandsConfig.RegisterCommands<PingCommand>(1006058186136096798);
            //SlashCommandsConfig.RegisterCommands<PostCommand>(1006058186136096798);
            //SlashCommandsConfig.RegisterCommands<PostContractCommand>(1006058186136096798);
            //SlashCommandsConfig.RegisterCommands<PromoteCommand>(1006058186136096798);
            //SlashCommandsConfig.RegisterCommands<RanksCommand>(1006058186136096798);
            //SlashCommandsConfig.RegisterCommands<RemoveItemCommand>(1006058186136096798);
            //SlashCommandsConfig.RegisterCommands<SetCurrencyCommand>(1006058186136096798);
            //SlashCommandsConfig.RegisterCommands<SetDonateCommand>(1006058186136096798);
            //SlashCommandsConfig.RegisterCommands<SetFlightsCommand>(1006058186136096798);
            //SlashCommandsConfig.RegisterCommands<SetLastSeenCommand>(1006058186136096798);
            //SlashCommandsConfig.RegisterCommands<SetStreakCommand>(1006058186136096798);
            //SlashCommandsConfig.RegisterCommands<TestCommand>(1006058186136096798);
            //SlashCommandsConfig.RegisterCommands<UpdateMessageCommand>(1006058186136096798);
            //SlashCommandsConfig.RegisterCommands<VipCommand>(1006058186136096798);


        }

        private async Task Client_GuildMemberJoined(DiscordClient client, GuildMemberAddedEventArgs args)
        {

            if (args.Guild.Id != 734214744818581575 && args.Guild.Id != 1006058186136096798)
                return;

            MemberJoinedEvent.MemberJoined(args);
        }

        private Task Client_InteractionCreated(DiscordClient sender, InteractionCreatedEventArgs args)
        {

            if (args.Interaction.Guild.Id != 734214744818581575 && args.Interaction.Guild.Id != 1006058186136096798)
                return Task.CompletedTask;

            if (args.Interaction.Type == DiscordInteractionType.Component)
            {
                args.Interaction.CreateResponseAsync(DiscordInteractionResponseType.DeferredMessageUpdate);
                ButtonPressedEvent.ButtonPressed(args);
            }

            return Task.CompletedTask;
        }

        private Task Client_ModalSubmitted(DiscordClient sender, ModalSubmittedEventArgs args)
        {
            //args.Interaction.CreateResponseAsync(DiscordInteractionResponseType.DeferredMessageUpdate);

            return Task.CompletedTask;
        }

        private Task Client_MessageCreated(DiscordClient sender, MessageCreatedEventArgs args)
        {

            MessageCreatedEvent.MessageCreated(args);

            return Task.CompletedTask;
        }

        public async Task RunAsync()
        {
            DiscordActivity status = new("with fire", DiscordActivityType.Playing);

            await Client.ConnectAsync(status, DiscordUserStatus.Online);


            IC = new ItemController(await Client.GetGuildAsync(1006058186136096798));
            BC = new BondsController(await Client.GetGuildAsync(1006058186136096798));
            MC = new MessageController(await Client.GetGuildAsync(1006058186136096798));
            IDC = new IDController(await Client.GetGuildAsync(1006058186136096798));
            RC = new RankController(await Client.GetGuildAsync(1006058186136096798));
            CC = new ContractController(await Client.GetGuildAsync(1006058186136096798), await Client.GetGuildAsync(1006058186136096798)); //TODO: update second guild to RVN 
            TC = new TimerController(await Client.GetGuildAsync(1006058186136096798)); //TODO: update to Ravenspear ID
            EC = new EventController(await Client.GetGuildAsync(1006058186136096798));
            OQC = new OnboardingQuestController(await Client.GetGuildAsync(1006058186136096798), await Client.GetGuildAsync(1006058186136096798)); //TODO: update second guild to RVN );
            WEMC = new WeeklyEventManagerController(await Client.GetGuildAsync(1006058186136096798), await Client.GetGuildAsync(1006058186136096798)); //TODO: update First guild to RVN );

            await Task.Delay(-1);
        }

        private Task Client_SessionCreated(DiscordClient sender, SessionCreatedEventArgs args)
        {
            return Task.CompletedTask;
        }
    }
}
