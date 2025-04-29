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
        UserActionController UAC;
        GuildEventController GEC;
        PRatingController PRC;
        bool started = false;


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

            CBuilder.ConfigureExtraFeatures
                (
                b => b
                .AbsoluteMessageCacheExpiration = new TimeSpan(5,0,0,0)
                );

            CBuilder.ConfigureEventHandlers
            (
                b => b
                .HandleMessageCreated(Client_MessageCreated)
                .HandleComponentInteractionCreated(Client_InteractionCreated)
                .HandleGuildMemberAdded(Client_GuildMemberJoined)
                .HandleMessageUpdated(MessageEdited)
                .HandleMessageDeleted(MessageDeleted)
                .HandleGuildMemberRemoved(MemberLeft)
                .HandleScheduledGuildEventCreated(EventCreated)
                .HandleScheduledGuildEventUpdated(EventUpdated)
                .HandleScheduledGuildEventCompleted(EventCompleted)
                .HandleVoiceStateUpdated(UserVoiceUpdated)
                .HandleModalSubmitted(Client_ModalSubmitted)
                .HandleZombied(Client_Zombied)
                .HandleSocketClosed(Client_SocketClosed)
                .HandleSocketOpened(Client_SocketOpened)
            );


            CBuilder.UseCommands(

                // we register our commands here
                (e, extension) =>
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
                                           typeof(ClearQuestCommand), typeof(SayCommand),
                                           typeof(UpdateMessageCommand), typeof(VipCommand),
                                           typeof(SortInventoryCommand), typeof(PRatingCommand),
                                           typeof(AdjustPRatingCommand), typeof(GetPRatingCommand),
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
                    // The default value, however it's shown here for clarity
                    RegisterDefaultCommandProcessors = true
                }
            );




            Client = CBuilder.Build();

        }

        private async Task Client_SocketOpened(DiscordClient client, SocketOpenedEventArgs args)
        {
            Console.WriteLine("Socket opened.");
        }

        private async Task Client_SocketClosed(DiscordClient client, SocketClosedEventArgs args)
        {
            Console.WriteLine("Socket closed. \nMessage: " + args.CloseMessage + "\nCode: " + args.CloseCode);
        }

        private async Task Client_Zombied(DiscordClient client, ZombiedEventArgs args)
        {
            Console.WriteLine("We zombied, Failures: " + args.Failures);
        }

        private async Task UserVoiceUpdated(DiscordClient client, VoiceStateUpdatedEventArgs args)
        {
            if (args.Guild.Id != 734214744818581575 && args.Guild.Id != 1006058186136096798)
                return;
            UserVoiceUpdatedEvent.UserVoiceUpdated(args);
        }

        private async Task EventCompleted(DiscordClient client, ScheduledGuildEventCompletedEventArgs args)
        {
            if (args.Event.Guild.Id != 734214744818581575 && args.Event.Guild.Id != 1006058186136096798)
                return;

            GuildEventCompletedEvent.GuildEventCompleted(args);
        }

        private async Task EventUpdated(DiscordClient client, ScheduledGuildEventUpdatedEventArgs args)
        {
            if (args.EventAfter.Guild.Id != 734214744818581575 && args.EventAfter.Guild.Id != 1006058186136096798)
                return;

            GuildEventUpdatedEvent.GuildEventUpdated(args);
        }

        private async Task EventCreated(DiscordClient client, ScheduledGuildEventCreatedEventArgs args)
        {
            if (args.Guild.Id != 734214744818581575 && args.Guild.Id != 1006058186136096798)
                return;

            GuildEventCreatedEvent.GuildEventCreated(args);
        }

        private async Task MemberLeft(DiscordClient client, GuildMemberRemovedEventArgs args)
        {
            if (args.Guild.Id != 734214744818581575 && args.Guild.Id != 1006058186136096798)
                return;

            MemberLeftEvent.MemberLeft(args);
        }

        private async Task MessageDeleted(DiscordClient client, MessageDeletedEventArgs args)
        {
            if (args.Guild.Id != 734214744818581575 && args.Guild.Id != 1006058186136096798)
                return;

            MessageDeletedEvent.MessageDeleted(args);
        }

        private async Task MessageEdited(DiscordClient client, MessageUpdatedEventArgs args)
        {
            if (args.Guild.Id != 734214744818581575 && args.Guild.Id != 1006058186136096798)
                return;

            MessageEditedEvent.MessageEdited(args);
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
                if(!args.Interaction.Data.CustomId.Contains("EditBondsPayout"))
                    args.Interaction.CreateResponseAsync(DiscordInteractionResponseType.DeferredMessageUpdate);
                ButtonPressedEvent.ButtonPressed(args);
            }

            return Task.CompletedTask;
        }

        private Task Client_ModalSubmitted(DiscordClient sender, ModalSubmittedEventArgs args)
        {
            if (args.Interaction.Guild.Id != 734214744818581575 && args.Interaction.Guild.Id != 1006058186136096798)
                return Task.CompletedTask;

            args.Interaction.CreateResponseAsync(DiscordInteractionResponseType.DeferredMessageUpdate);

            ModalSubmittedEvent.ModalSubmitted(args);

            return Task.CompletedTask;
        }

        private Task Client_MessageCreated(DiscordClient sender, MessageCreatedEventArgs args)
        {
            if (args.Guild.Id != 734214744818581575 && args.Guild.Id != 1006058186136096798)
                return Task.CompletedTask;

            MessageCreatedEvent.MessageCreated(args);

            return Task.CompletedTask;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("RunAsync");
            if (started)
            {
                Console.WriteLine("Reconnecting..");
                await Client.ReconnectAsync();
                await Client.UpdateStatusAsync(new DiscordActivity("Lasing for friends!", DiscordActivityType.Custom));
                return;
            }

            await Client.ConnectAsync(new DiscordActivity("Taking runway 32 for takeoff!"), DiscordUserStatus.Online);

            DiscordGuild DC_Lair = await Client.GetGuildAsync(1006058186136096798);
            DiscordGuild RVN = await Client.GetGuildAsync(734214744818581575); //TODO: change to RVN

            IC = new ItemController(DC_Lair);
            BC = new BondsController(RVN, DC_Lair);
            MC = new MessageController(RVN);
            IDC = new IDController(RVN);
            RC = new RankController(DC_Lair);
            CC = new ContractController(DC_Lair, RVN);
            TC = new TimerController(RVN);
            EC = new EventController(RVN);
            OQC = new OnboardingQuestController(DC_Lair, RVN);
            WEMC = new WeeklyEventManagerController(RVN, DC_Lair);
            UAC = new UserActionController(RVN);
            GEC = new GuildEventController(RVN);
            PRC = new PRatingController(RVN);

            await Client.UpdateStatusAsync(new DiscordActivity("Lasing for friends!", DiscordActivityType.Custom));

            started = true;

            StartLatencyHeartbeat(Client);

            await Task.Delay(-1);
        }

        private Timer _latencyTimer;

        public void StartLatencyHeartbeat(DiscordClient client)
        {
            Console.WriteLine("Starting heartbeat");
            _latencyTimer = new Timer(async _ =>
            {
                await SendLatencyHeartbeat(client);
            }, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        }
        public async Task SendLatencyHeartbeat(DiscordClient client)
        {
            if (client.CurrentUser.Presence.Activity != new DiscordActivity("Lasing for friends!", DiscordActivityType.Custom))
                await client.UpdateStatusAsync(new DiscordActivity("Lasing for friends!", DiscordActivityType.Custom));
            var channel = await client.GetGuildAsync(1006058186136096798).Result.GetChannelAsync(1362553389770084562); // Your channel ID
            int latency = (int)client.GetConnectionLatency(734214744818581575).TotalMilliseconds;
            string message = $"🫀 Heartbeat | Latency: {latency}ms | Time: {DateTime.Now:HH:mm:ss}";
            await channel.SendMessageAsync(message);
            Console.WriteLine(message);
        }

        private Task Client_SessionCreated(DiscordClient sender, SessionCreatedEventArgs args)
        {
            return Task.CompletedTask;
        }
    }
}
