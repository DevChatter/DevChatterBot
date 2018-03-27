using System;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public class AddCommandCommand : SimpleCommand
    {
        private readonly IRepository _repository;

        public AddCommandCommand(IRepository repository)
        {
            _repository = repository;
            CommandText = "AddCommand";
            RoleRequired = UserRole.Mod;
        }

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            try
            {
                // !AddCommand Twitter "https://twitter.com/DevChatter_" Everyone
                SimpleCommand command = eventArgs.Arguments.ToSimpleCommand();

                if (_repository.Single(CommandPolicy.ByCommandText(command.CommandText)) != null)
                {
                    chatClient.SendMessage($"There's already a command using !{command.CommandText}");
                    return;
                }

                chatClient.SendMessage($"Adding a !{command.CommandText} command for {command.RoleRequired}. It will respond with {command.StaticResponse}.");

                _repository.Create(command);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}