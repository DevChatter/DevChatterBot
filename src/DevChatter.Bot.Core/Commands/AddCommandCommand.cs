using System;
using DevChatter.Bot.Core.ChatSystems;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Model;

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

                chatClient.SendMessage($"You are trying to make a !{command.CommandText} command for {command.RoleRequired} responding with {command.StaticResponse}.");

                if (_repository.Single(CommandPolicy.ByCommandText(command.CommandText)) == null)
                {
                    _repository.Create(command);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}