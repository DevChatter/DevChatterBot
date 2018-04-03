using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public class HelpCommand : IBotCommand
    {
        private readonly List<IBotCommand> _allCommands;
        public UserRole RoleRequired { get; }
        public string CommandText { get; }
        public string HelpText { get; }

        public HelpCommand(List<IBotCommand> allCommands)
        {
            _allCommands = allCommands;
            CommandText = "help";
            RoleRequired = UserRole.Everyone;
            HelpText = "I think you figured this out already...";
        }

        public void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            if (eventArgs.Arguments.Count == 0)
            {
                ShowAvailableCommands(chatClient, eventArgs.ChatUser);
                return;
            }

            string argOne = eventArgs?.Arguments?.ElementAtOrDefault(0);

            if (argOne == "?")
            {
                chatClient.SendMessage($"Use !help to see available commands. To request help for a specific command just type !help [commandname] example: !help hangman");
                return;
            }

            if (argOne == "↑, ↑, ↓, ↓, ←, →, ←, →, B, A, start, select")
            {
                chatClient.SendMessage("Please be sure to drink your ovaltine.");
            }

            IBotCommand requestedCommand = _allCommands.SingleOrDefault(x =>
                x.CommandText.Equals(argOne, StringComparison.InvariantCultureIgnoreCase));

            if (requestedCommand != null)
            {
                chatClient.SendMessage(requestedCommand.HelpText);
            }
        }

        private void ShowAvailableCommands(IChatClient chatClient, ChatUser chatUser)
        {
            var listOfCommands = _allCommands.Where(chatUser.CanUserRunCommand).Select(x => $"!{x.CommandText}").ToList();

            string stringOfCommands = string.Join(", ", listOfCommands);
            chatClient.SendMessage($"These are the commands that {chatUser.DisplayName} is allowed to run: ({stringOfCommands})");

        }
    }
}