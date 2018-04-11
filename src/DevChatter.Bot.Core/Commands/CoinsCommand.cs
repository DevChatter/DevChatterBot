using System;
using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public class CoinsCommand : BaseCommand
    {
        private readonly IRepository _repository;

        public CoinsCommand(IRepository repository)
            : base(repository, UserRole.Everyone)
        {
            _repository = repository;
        }

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
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

                chatClient.SendMessage($"{userToCheck} has {chatUser.Tokens} tokens!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
