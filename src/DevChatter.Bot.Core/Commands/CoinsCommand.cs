using System;
using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Model;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public class CoinsCommand : SimpleCommand
    {
        private readonly IRepository _repository;

        public CoinsCommand(IRepository repository)
        {
            _repository = repository;
            CommandText = "coins";
            RoleRequired = UserRole.Everyone;
        }

        public override void Process(IChatClient triggeringClient, CommandReceivedEventArgs eventArgs)
        {
            try
            {
                string userToCheck = eventArgs.ChatUser.DisplayName;
                string specifiedUser = eventArgs.Arguments?.FirstOrDefault()?.NoAt();

                if (specifiedUser != null)
                {
                    userToCheck = specifiedUser;
                }

                ChatUser chatUser = _repository.Single(ChatUserPolicy.ByDisplayName(userToCheck));

                triggeringClient.SendMessage($"{userToCheck} has {chatUser.Tokens} tokens!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}