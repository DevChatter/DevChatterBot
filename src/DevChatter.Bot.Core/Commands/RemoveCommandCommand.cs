using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public class RemoveCommandCommand : BaseCommand
    {
        private readonly IRepository _repository;
        private readonly List<IBotCommand> _allCommands;

        public RemoveCommandCommand(IRepository repository, List<IBotCommand> allCommands)
            : base(UserRole.Mod)
        {
            _repository = repository;
            _allCommands = allCommands;
        }

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            try
            {
                // !RemoveCommand Twitter

                string commandText = eventArgs.Arguments?[0];

                SimpleCommand command = _repository.Single(CommandPolicy.ByCommandText(commandText));
                if (command == null)
                {
                    chatClient.SendMessage($"I didn't find a !{commandText} command.");
                    return;
                }

                chatClient.SendMessage($"Removing the !{commandText} command.");

                _repository.Remove(command);
                IBotCommand botCommand = _allCommands.SingleOrDefault(x => x.ShouldExecute(commandText));
                if (botCommand != null)
                {
                    _allCommands.Remove(botCommand);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}