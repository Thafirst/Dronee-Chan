﻿using Dronee_Chan_2.Discord_Bot.Enums;
using Dronee_Chan_2.Discord_Bot.Events;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Controllers
{
    internal class WeeklyEventManagerController
    {
        DiscordGuild RVN;
        DiscordGuild DC_Lair;
        Dictionary<string, string> EventDetails = new Dictionary<string, string>();

        public WeeklyEventManagerController(DiscordGuild rVN, DiscordGuild dC_Lair)
        {
            RVN = rVN;
            DC_Lair = dC_Lair;

            ButtonPressedEvent.ButtonPressedEventRaised += ButtonPressedEvent_ButtonPressedEventRaised;
        }

        private void ButtonPressedEvent_ButtonPressedEventRaised(DSharpPlus.EventArgs.InteractionCreatedEventArgs args)
        {
            if (!args.Interaction.Data.CustomId.Contains("EventManagement"))
                return;

            if (args.Interaction.Channel.Id != 1104088644106592348) //TODO: Change to RVN Event Management channel
                return;

            var buttonID = args.Interaction.Data.CustomId;

            switch(Enum.Parse(typeof(EventManagementEnums),buttonID))
            {
                case EventManagementEnums.NewEventManagement:
                    CreateNewEvent();
                    break;
                case EventManagementEnums.EditEventManagement:
                    EditEvent(args.Interaction.Message);
                    break;
                case EventManagementEnums.EventEventManagement:
                    SetEvent(args.Interaction.Message.Id, args.Interaction.Data.Values[0]);
                    break;
                case EventManagementEnums.DayEventManagement:
                    SetDay(args.Interaction.Message.Id, args.Interaction.Data.Values[0]);
                    break;
                case EventManagementEnums.TimeEventManagement:
                    SetTime(args.Interaction.Message.Id, args.Interaction.Data.Values[0]);
                    break;
                case EventManagementEnums.FinalizeEventManagement:
                    FinalizeEvent(args.Interaction.Message);
                    break;
                case EventManagementEnums.DeleteEventManagement:
                    DeleteEvent(args.Interaction.Message);
                    break;
            }
        }

        private async void CreateNewEvent()
        {
            DiscordMessage message = await RVN.GetChannelAsync(1104088644106592348).Result.SendMessageAsync("Fill out Event");
            AddDropdown(message);
        }

        private void EditEvent(DiscordMessage? discordMessage)
        {
            AddDropdown(discordMessage);
        }

        private void SetEvent(ulong messageID, string value)
        {
            AddToEvent(messageID + "Event", value);
        }

        private void SetDay(ulong messageID, string value)
        {
            AddToEvent(messageID + "Day", value);
        }

        private void SetTime(ulong messageID, string value)
        {
            AddToEvent(messageID + "Time", value);
        }

        private async void FinalizeEvent(DiscordMessage? discordMessage)
        {
            if (!EventDetails.ContainsKey(discordMessage.Id + "Event") || !EventDetails.ContainsKey(discordMessage.Id + "Day") || !EventDetails.ContainsKey(discordMessage.Id + "Time"))
                return;

            string message = EventDetails[discordMessage.Id + "Event"] + "\n" + EventDetails[discordMessage.Id + "Day"] + "\n" + EventDetails[discordMessage.Id + "Time"];

            var editButton = new DiscordButtonComponent(DiscordButtonStyle.Primary, EventManagementEnums.EditEventManagement.ToString(), "Edit");
            var deleteButton = new DiscordButtonComponent(DiscordButtonStyle.Danger, EventManagementEnums.DeleteEventManagement.ToString(), "Delete");

            var builder = new DiscordMessageBuilder()
                .WithContent(message)
                .AddComponents([editButton, deleteButton]);

            await discordMessage.ModifyAsync(builder);

            EventDetails.Remove(discordMessage.Id + "Event");
            EventDetails.Remove(discordMessage.Id + "Day");
            EventDetails.Remove(discordMessage.Id + "Time");
        }

        private async void DeleteEvent(DiscordMessage? discordMessage)
        {
            await discordMessage.DeleteAsync();
        }

        private void AddToEvent(string key, string value)
        {
            if (EventDetails.ContainsKey(key))
            {
                EventDetails[key] = value;
                return;
            }
            EventDetails.Add(key, value);
        }

        private async void AddDropdown(DiscordMessage message)
        {
            string eventsMessage = DC_Lair.GetChannelAsync(1104199917813112882) //Events Channel
                .Result.GetMessageAsync(1104200190551932928).Result.Content;



            var eventOptions = new string[] {"0","0","0"};
            if (message.Content != "Fill out Event")
            {
                eventOptions = message.Content.Split('\n');
                AddToEvent(message.Id + "Event", eventOptions[0]);
                AddToEvent(message.Id + "Day", eventOptions[1]);
                AddToEvent(message.Id + "Time", eventOptions[2]);
            }

            var events = new List<DiscordSelectComponentOption>();

            foreach (string str in eventsMessage.Split('\n'))
            {
                var temp = str.Split(':')[0];
                events.Add(new DiscordSelectComponentOption(temp, temp));
            }

            var days = new List<DiscordSelectComponentOption>(){
                new DiscordSelectComponentOption(
                    "Monday",
                    "Monday"),

                new DiscordSelectComponentOption(
                    "Tuesday",                  
                    "Tuesday"),

                new DiscordSelectComponentOption(
                    "Wedensday",
                    "Wedensday"),

                new DiscordSelectComponentOption(
                    "Thursday",
                    "Thursday"),

                new DiscordSelectComponentOption(
                    "Friday",
                    "Friday"),

                new DiscordSelectComponentOption(
                    "Saturday",
                    "Saturday"),

                new DiscordSelectComponentOption(
                    "Sunday",
                    "Sunday")
            };

            var time = new List<DiscordSelectComponentOption>(){
                new DiscordSelectComponentOption(
                    "15.00", "15.00"),
                new DiscordSelectComponentOption(
                    "15.30", "15.30"),
                new DiscordSelectComponentOption(
                    "16.00", "16.00"),
                new DiscordSelectComponentOption(
                    "16.30", "16.30"),
                new DiscordSelectComponentOption(
                    "17.00", "17.00"),
                new DiscordSelectComponentOption(
                    "17.30", "17.30"),
                new DiscordSelectComponentOption(
                    "18.00", "18.00"),
                new DiscordSelectComponentOption(
                    "18.30", "18.30"),
                new DiscordSelectComponentOption(
                    "19.00", "19.00"),
                new DiscordSelectComponentOption(
                    "19.30", "19.30"),
                new DiscordSelectComponentOption(
                    "20.00", "20.00"),
                new DiscordSelectComponentOption(
                    "20.30", "20.30"),
                new DiscordSelectComponentOption(
                    "21.00", "21.00"),
                new DiscordSelectComponentOption(
                    "21.30", "21.30"),
                new DiscordSelectComponentOption(
                    "22.00", "22.00"),
                new DiscordSelectComponentOption(
                    "22.30", "22.30"),
                new DiscordSelectComponentOption(
                    "23.00", "23.00"),
                new DiscordSelectComponentOption(
                    "23.30", "23.30"),
                new DiscordSelectComponentOption(
                    "00.00", "00.00"),
                new DiscordSelectComponentOption(
                    "00.30", "00.30"),
            };

            string placeholder = "Select an event";
            if (eventOptions[0] != "0")
                placeholder = eventOptions[0];
            var EventDropdown = new DiscordSelectComponent(EventManagementEnums.EventEventManagement.ToString(), placeholder, events);

            placeholder = "Select an day";
            if (eventOptions[1] != "0")
                placeholder = eventOptions[1];
            var DayDropdown = new DiscordSelectComponent(EventManagementEnums.DayEventManagement.ToString(), placeholder, days);

            placeholder = "Select an time";
            if (eventOptions[2] != "0")
                placeholder = eventOptions[2];
            var TimeDropdown = new DiscordSelectComponent(EventManagementEnums.TimeEventManagement.ToString(), placeholder, time);

            

            var button = new DiscordButtonComponent(DiscordButtonStyle.Primary, EventManagementEnums.FinalizeEventManagement.ToString(), "Finalize");

            var builder = new DiscordMessageBuilder()
                .WithContent(message.Content)
                .AddComponents(EventDropdown)
                .AddComponents(DayDropdown)
                .AddComponents(TimeDropdown)
                .AddComponents(button);

            await message.ModifyAsync(builder);
        }
    }
}
