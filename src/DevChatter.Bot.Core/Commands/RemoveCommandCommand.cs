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

        public RemoveCommandCommand(IRepository repository)
            : base(repository, UserRole.Mod)
        {
            _repository = repository;
        }

        public IList<IBotCommand> AllCommands { get; set; }

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
                IBotCommand botCommand = AllCommands.SingleOrDefault(x => x.ShouldExecute(commandText));
                if (botCommand != null)
                {
                    AllCommands.Remove(botCommand);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}