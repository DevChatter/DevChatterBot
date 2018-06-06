using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;
using System;
using System.Linq;

namespace DevChatter.Bot.Core.Commands
{
    public class CoinsCommand : BaseCommand
    {
        private readonly ILoggerAdapter<CoinsCommand> _logger;

        public CoinsCommand(IRepository repository, ILoggerAdapter<CoinsCommand> logger)
            : base(repository, UserRole.Everyone)
        {
            _logger = logger;
            HelpText = "Use \"!coins\" to see your current coin total, or use \"!coins SomeoneElse\" to see another user's coin total.";
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            string userToCheck = eventArgs?.ChatUser?.DisplayName;
            try
            {
                string specifiedUser = eventArgs?.Arguments?.FirstOrDefault()?.NoAt();

                if (specifiedUser != null)
                {
                    userToCheck = specifiedUser;
                }

                ChatUser chatUser = Repository.Single(ChatUserPolicy.ByDisplayName(userToCheck));

                chatClient.SendMessage($"{userToCheck} has {chatUser?.Tokens ?? 0} tokens!");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to look up coins for {userToCheck}.");
            }
        }
    }
}
