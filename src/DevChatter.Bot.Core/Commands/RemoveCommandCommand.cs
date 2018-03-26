using System;
using DevChatter.Bot.Core.ChatSystems;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Model;

namespace DevChatter.Bot.Core.Commands
{
    public class RemoveCommandCommand : SimpleCommand
    {
        private readonly IRepository _repository;

        public RemoveCommandCommand(IRepository repository)
        {
            _repository = repository;
            CommandText = "RemoveCommand";
            RoleRequired = UserRole.Mod;
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
                }

                chatClient.SendMessage($"Removing the !{commandText} command.");

                _repository.Remove(command);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}