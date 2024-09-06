using Dronee_Chan_2.Discord_Bot.Enums;
using Dronee_Chan_2.Discord_Bot.Events;
using Dronee_Chan_2.Discord_Bot.Objects.UserObjects;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Controllers
{
    internal class OnboardingQuestController
    {
        DiscordGuild RVN;
        DiscordGuild DCLair;

        public OnboardingQuestController(DiscordGuild dCLair, DiscordGuild rVN)
        {
            RVN = rVN;
            DCLair = dCLair;

            StartOnboardingQuestEvent.StartOnboardingQuestEventRaised += StartOnboardingQuestEvent_StartOnboardingQuestEventRaised;
            ButtonPressedEvent.ButtonPressedEventRaised += ButtonPressedEvent_ButtonPressedEventRaised;
            MemberJoinedEvent.MemberJoinedEventRaised += MemberJoinedEvent_MemberJoinedEventRaised;
        }

        private async void MemberJoinedEvent_MemberJoinedEventRaised(GuildMemberAddedEventArgs args)
        {
            User user = await EventManager.LoadUser(args.Member.Id);
            if (user.OnboardingQuestMistakes >= 2)
                DenyUser(user);
        }

        private void ButtonPressedEvent_ButtonPressedEventRaised(InteractionCreatedEventArgs args)
        {

            if (!args.Interaction.Data.CustomId.Contains("OnboardingQuest"))
                return;

            switch (args.Interaction.Data.ComponentType)
            {
                case DiscordComponentType.Button:
                    HandleButton(args.Interaction.Data.CustomId, args.Interaction.User.Id, args.Interaction.Channel);
                    break;
                case DiscordComponentType.StringSelect:
                    HandleDropdown(args.Interaction.Data.CustomId, args.Interaction.Data.Values, args.Interaction.User.Id);
                    break;
            }

        }

        private async void HandleDropdown(string ID, string[] values, ulong userID)
        {
            User user = await EventManager.LoadUser(userID);

            if (user.OnboardingQuestChoices.ContainsKey(ID))
                user.OnboardingQuestChoices[ID] = values[0];
            else
                user.OnboardingQuestChoices.Add(ID, values[0]);

            EventManager.SaveUser(user);
        }

        private void HandleButton(string ID, ulong userID, DiscordChannel channel)
        {
            if (ID == OnboardingQuestEnums.FirstContinueButtonOnboardingQuest.ToString())
                QuestOneContinue(userID, channel);
            if (ID == OnboardingQuestEnums.SecondContinueButtonOnboardingQuest.ToString())
                QuestTwoContinue(userID, channel);
            if (ID == OnboardingQuestEnums.ThirdContinueButtonOnboardingQuest.ToString())
                QuestThreeContinue(userID, channel);
            if (ID == OnboardingQuestEnums.FourthContinueButtonOnboardingQuest.ToString())
                QuestFourContinue(userID, channel);
            if (ID == OnboardingQuestEnums.SubmitButtonOnboardingQuest.ToString())
                QuestEnd(userID, channel);
        }

        private async void QuestOneContinue(ulong userID, DiscordChannel channel)
        {
            User user = await EventManager.LoadUser(userID);

            if (!user.OnboardingQuestChoices.ContainsKey(OnboardingQuestEnums.FirstQuestOnboardingQuest.ToString()) ||
                user.OnboardingQuestChoices[OnboardingQuestEnums.FirstQuestOnboardingQuest.ToString()] != "Clearance Level 1")
            {
                user.OnboardingQuestMistakes++;
                EventManager.SaveUser(user);
            }

            ClearChannelMessages(channel);

            PostQuest_2(channel, user.DiscordUUID);
        }

        private async void QuestTwoContinue(ulong userID, DiscordChannel channel)
        {
            User user = await EventManager.LoadUser(userID);

            if (!user.OnboardingQuestChoices.ContainsKey(OnboardingQuestEnums.SecondQuestOnboardingQuest.ToString()) ||
                user.OnboardingQuestChoices[OnboardingQuestEnums.SecondQuestOnboardingQuest.ToString()] != "R$ Currency")
            {
                user.OnboardingQuestMistakes++;
                EventManager.SaveUser(user);
            }

            if (ValidateMistakes(user))
            {
                DenyUser(user, channel);
                return;
            }

            ClearChannelMessages(channel);

            PostQuest_3(channel, user.DiscordUUID);
        }

        private async void QuestThreeContinue(ulong userID, DiscordChannel channel)
        {
            User user = await EventManager.LoadUser(userID);

            if (!user.OnboardingQuestChoices.ContainsKey(OnboardingQuestEnums.ThirdQuestOnboardingQuest.ToString()) ||
                user.OnboardingQuestChoices[OnboardingQuestEnums.ThirdQuestOnboardingQuest.ToString()] != "After 3 months")
            {
                user.OnboardingQuestMistakes++;
                EventManager.SaveUser(user);
            }

            if (ValidateMistakes(user))
            {
                DenyUser(user, channel);
                return;
            }

            ClearChannelMessages(channel);

            PostQuest_4(channel, user.DiscordUUID);
        }

        private async void QuestFourContinue(ulong userID, DiscordChannel channel)
        {
            User user = await EventManager.LoadUser(userID);

            if (!user.OnboardingQuestChoices.ContainsKey(OnboardingQuestEnums.FourthQuestOnboardingQuest.ToString()) ||
                user.OnboardingQuestChoices[OnboardingQuestEnums.FourthQuestOnboardingQuest.ToString()] != "Three Infractions")
            {
                user.OnboardingQuestMistakes++;
                EventManager.SaveUser(user);
            }

            if (ValidateMistakes(user))
            {
                DenyUser(user, channel);
                return;
            }

            ClearChannelMessages(channel);

            PostQuest_End(channel, user.DiscordUUID);
        }

        private async void QuestEnd(ulong userID, DiscordChannel channel)
        {
            DiscordMember discordUser = await RVN.GetMemberAsync(userID);

            DiscordRole memberRole = await RVN.GetRoleAsync(1230931404775489576); //TODO: Change to Member role
            DiscordRole newRole = await RVN.GetRoleAsync(1006063552941002842); //TODO: Change to New role
            DiscordRole questingRole = await RVN.GetRoleAsync(1230931522291241040); //TODO: Change to Questing role

            await channel.DeleteAsync();

            await discordUser.RevokeRoleAsync(questingRole);
            await discordUser.GrantRoleAsync(newRole);
            await discordUser.GrantRoleAsync(memberRole);
        }

        private async void DenyUser(User user)
        {
            DenyUser(user, null);
        }

        private async void DenyUser(User user, DiscordChannel discordChannel)
        {
            if(discordChannel != null)
                await discordChannel.DeleteAsync();

            DiscordMember discordUser = await RVN.GetMemberAsync(user.DiscordUUID);
            DiscordRole questingRole = await RVN.GetRoleAsync(1230931522291241040); //TODO: Change to Questing role
            DiscordRole denyRole = await RVN.GetRoleAsync(1277522729540653087); //TODO: Change to Questing role
            await discordUser.RevokeRoleAsync(questingRole);
            await discordUser.GrantRoleAsync(denyRole);
        }

        private bool ValidateMistakes(User user)
        {
            if(user.OnboardingQuestMistakes >= 2)
                return true;
            return false;
        }

        private async void StartOnboardingQuestEvent_StartOnboardingQuestEventRaised(ulong UUID)
        {

            StartOnboardingQuest(await RVN.GetMemberAsync(UUID));
        }

        private async void StartOnboardingQuest(DiscordMember user)
        {
            //Thread.Sleep(2000);

            DiscordOverwriteBuilder EveryoneDOB = new DiscordOverwriteBuilder(RVN.EveryoneRole).Deny(DiscordPermissions.All);
            DiscordOverwriteBuilder UserDOB = new DiscordOverwriteBuilder(user).Allow(DiscordPermissions.AccessChannels).Allow(DiscordPermissions.ReadMessageHistory);

            DiscordChannel channel = await RVN.CreateTextChannelAsync(user.Username + " " + user.Id, await RVN.GetChannelAsync(1006058186677170247) //TODO: change to ID: // Onboarding Category
                , overwrites: [EveryoneDOB, UserDOB]);

            PostQuest_1(channel, user.Id);

            await user.GrantRoleAsync(await RVN.GetRoleAsync(1230931522291241040)); //TODO: Change to ID: //Questing Role
        }

        private async void PostQuest_1(DiscordChannel channel, ulong userID)
        {
            string part1 = DCLair   //Dronee-Chan's Lair Guild
                .GetChannelAsync(1105543871510884422).Result   //Onboarding-Quests Channel
                .GetMessageAsync(1105543918952656959)  //First onboarding quest 1 messsage
                .Result.Content;

            string part2 = DCLair   //Dronee-Chan's Lair Guild
                .GetChannelAsync(1105543871510884422).Result   //Onboarding-Quests Channel
                .GetMessageAsync(1105543964074971247)  //Second onboarding quest 1 messsage
                .Result.Content;

            var options = new List<DiscordSelectComponentOption>(){
                new DiscordSelectComponentOption(
                    "Clearance Level 0",
                    "Clearance Level 0"),

                new DiscordSelectComponentOption(
                    "Clearance Level 1",                  //CORRECT
                    "Clearance Level 1"),

                new DiscordSelectComponentOption(
                    "Clearance Level 2",
                    "Clearance Level 2"),

                new DiscordSelectComponentOption(
                    "Clearance Level 3",
                    "Clearance Level 3"),

                new DiscordSelectComponentOption(
                    "Clearance Level 4",
                    "Clearance Level 4")
            };
            var dropdown = new DiscordSelectComponent(OnboardingQuestEnums.FirstQuestOnboardingQuest.ToString(), "Select an option", options);

            var button = new DiscordButtonComponent(DiscordButtonStyle.Primary, OnboardingQuestEnums.FirstContinueButtonOnboardingQuest.ToString(), "Continue");

            string path = await EventManager.GenerateIDC(await EventManager.LoadUser(userID));

            using (FileStream fs = File.OpenRead(path))
            {
                var builder = new DiscordMessageBuilder()
                    .WithContent(part1)
                    .AddFile(fs);

                await builder.SendAsync(channel);

                builder = new DiscordMessageBuilder()
                    .WithContent(part2)
                    .AddComponents(dropdown)
                    .AddComponents(button);

                await builder.SendAsync(channel);

            }
        }

        private async void PostQuest_2(DiscordChannel channel, ulong userID)
        {
            string part1 = DCLair   //Dronee-Chan's Lair Guild
                .GetChannelAsync(1105543871510884422).Result   //Onboarding-Quests Channel
                .GetMessageAsync(1105544028705013810)  //First onboarding quest 2 messsage
                .Result.Content;

            string part2 = DCLair   //Dronee-Chan's Lair Guild
                .GetChannelAsync(1105543871510884422).Result   //Onboarding-Quests Channel
                .GetMessageAsync(1106634903153549322)  //Second onboarding quest 2 messsage
                .Result.Content;

            var options = new List<DiscordSelectComponentOption>(){
                new DiscordSelectComponentOption(
                    "Clearance Level 2",
                    "Clearance Level 2"),

                new DiscordSelectComponentOption(
                    "Discord Nitro",
                    "Discord Nitro"),

                new DiscordSelectComponentOption(
                    "Access to the Ravenspear Shop",
                    "Access to the Ravenspear Shop"),

                new DiscordSelectComponentOption(
                    "R$ Currency",                  //CORRECT
                    "R$ Currency"),

                new DiscordSelectComponentOption(
                    "Aircraft liveries",
                    "Aircraft liveries")
            };
            var dropdown = new DiscordSelectComponent(OnboardingQuestEnums.SecondQuestOnboardingQuest.ToString(), "Select an option", options);

            var button = new DiscordButtonComponent(DiscordButtonStyle.Primary, OnboardingQuestEnums.SecondContinueButtonOnboardingQuest.ToString(), "Continue");

            var builder = new DiscordMessageBuilder()
                .WithContent(part1);

            await builder.SendAsync(channel);

            builder = new DiscordMessageBuilder()
                .WithContent(part2)
                .AddComponents(dropdown)
                .AddComponents(button);

            await builder.SendAsync(channel);

        }

        private async void PostQuest_3(DiscordChannel channel, ulong userID)
        {
            string part1 = DCLair   //Dronee-Chan's Lair Guild
                .GetChannelAsync(1105543871510884422).Result   //Onboarding-Quests Channel
                .GetMessageAsync(1105544073508560906)  //Onboarding quest 3 messsage
                .Result.Content;

            var options = new List<DiscordSelectComponentOption>(){
                new DiscordSelectComponentOption(
                    "After 2 weeks",
                    "After 2 weeks"),

                new DiscordSelectComponentOption(
                    "After 3 months",                  //CORRECT
                    "After 3 months"),

                new DiscordSelectComponentOption(
                    "By letting a staff member know",
                    "By letting a staff member know"),

                new DiscordSelectComponentOption(
                    "After 1 year",
                    "After 1 year"),

                new DiscordSelectComponentOption(
                    "Never",
                    "Never")
            };
            var dropdown = new DiscordSelectComponent(OnboardingQuestEnums.ThirdQuestOnboardingQuest.ToString(), "Select an option", options);

            var button = new DiscordButtonComponent(DiscordButtonStyle.Primary, OnboardingQuestEnums.ThirdContinueButtonOnboardingQuest.ToString(), "Continue");

            var builder = new DiscordMessageBuilder()
                .WithContent(part1)
                .AddComponents(dropdown)
                .AddComponents(button);

            await builder.SendAsync(channel);
        }

        private async void PostQuest_4(DiscordChannel channel, ulong userID)
        {
            string part1 = DCLair   //Dronee-Chan's Lair Guild
                .GetChannelAsync(1105543871510884422).Result   //Onboarding-Quests Channel
                .GetMessageAsync(1105544123697606696)  //Onboarding quest 4 messsage
                .Result.Content;

            var options = new List<DiscordSelectComponentOption>(){
                new DiscordSelectComponentOption(
                    "Three Infractions",                  //CORRECT
                    "Three Infractions"),

                new DiscordSelectComponentOption(
                    "Two Infractions",
                    "Two Infractions"),

                new DiscordSelectComponentOption(
                    "30 days",
                    "30 days"),

                new DiscordSelectComponentOption(
                    "One Infraction",
                    "One Infraction"),

                new DiscordSelectComponentOption(
                    "Three Months",
                    "Three Months")
            };
            var dropdown = new DiscordSelectComponent(OnboardingQuestEnums.FourthQuestOnboardingQuest.ToString(), "Select an option", options);

            var button = new DiscordButtonComponent(DiscordButtonStyle.Primary, OnboardingQuestEnums.FourthContinueButtonOnboardingQuest.ToString(), "Continue");

            var builder = new DiscordMessageBuilder()
                .WithContent(part1)
                .AddComponents(dropdown)
                .AddComponents(button);

            await builder.SendAsync(channel);
        }

        private async void PostQuest_End(DiscordChannel channel, ulong userID)
        {
            string part1 = DCLair   //Dronee-Chan's Lair Guild
                .GetChannelAsync(1105543871510884422).Result   //Onboarding-Quests Channel
                .GetMessageAsync(1105544153821102160)  //Onboarding quest end messsage
                .Result.Content;

            var button = new DiscordButtonComponent(DiscordButtonStyle.Primary, OnboardingQuestEnums.SubmitButtonOnboardingQuest.ToString(), "Submit");

            var builder = new DiscordMessageBuilder()
                .WithContent(part1)
                .AddComponents(button);

            await builder.SendAsync(channel);
        }

        private async void ClearChannelMessages(DiscordChannel discordChannel)
        {
            await foreach (DiscordMessage message in discordChannel.GetMessagesAsync())
            {
                await message.DeleteAsync();
            }
            Thread.Sleep(500);
        }
    }
}
