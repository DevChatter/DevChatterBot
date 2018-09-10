using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Core.Commands
{
    public class HelpCommand : BaseCommand
    {
        private readonly IServiceProvider _provider;

        public HelpCommand(IRepository repository, IServiceProvider provider)
            : base(repository, UserRole.Everyone)
        {
            _provider = provider;
            HelpText = "I think you figured this out already...";
        }

        private IList<IBotCommand> _allCommands;
        public IList<IBotCommand> AllCommands
        {
            get { return _allCommands ?? (_allCommands = _provider.GetService<IList<IBotCommand>>()); }
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            if (eventArgs.Arguments.Count == 0)
            {
                ShowAvailableCommands(chatClient, eventArgs.ChatUser);
                return;
            }

            string argOne = eventArgs.Arguments?.ElementAtOrDefault(0);

            if (argOne == "?")
            {
                chatClient.SendMessage(
                    "Use !help to see available commands. To request help for a specific command just type !help [commandname] example: !help hangman");
                return;
            }

            if (argOne == "↑, ↑, ↓, ↓, ←, →, ←, →, B, A, start, select")
            {
                chatClient.SendMessage("Please be sure to drink your ovaltine.");
                return;
            }

            bool isVerboseMode = false;
            if (argOne == "-v")
            {
                isVerboseMode = true;
                argOne = eventArgs.Arguments?.ElementAtOrDefault(1);
            }

            IBotCommand requestedCommand = AllCommands.SingleOrDefault(x => x.ShouldExecute(argOne, out _));

            if (requestedCommand != null)
            {
                if (isVerboseMode)
                {
                    chatClient.SendDirectMessage(eventArgs.ChatUser.DisplayName, requestedCommand.FullHelpText);
                }
                else
                {
                    chatClient.SendMessage(requestedCommand.HelpText);
                }
            }
        }

        private void ShowAvailableCommands(IChatClient chatClient, ChatUser chatUser)
        {
            var commands = AllCommands.Where(chatUser.CanRunCommand).Select(x => $"!{x.PrimaryCommandText}");
            string stringOfCommands = string.Join(", ", commands);

            string message =
                $"These are the commands that {chatUser.DisplayName} is allowed to run: ({stringOfCommands})";
            chatClient.SendMessage(message);
        }
    }
}
