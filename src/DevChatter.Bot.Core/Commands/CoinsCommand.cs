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
            : base(repository)
        {
            _logger = logger;
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            string userToCheck = eventArgs?.ChatUser?.DisplayName;
            try
            {
                //TODO: Sanitize this.
                //string specifiedUser = eventArgs?.Arguments?.FirstOrDefault()?.NoAt();

                //if (specifiedUser != null)
                //{
                //    userToCheck = specifiedUser;
                //}

                ChatUser chatUser = Repository.Single(ChatUserPolicy.ByDisplayName(userToCheck));

                chatClient.SendMessage($"User {userToCheck} has {chatUser?.Tokens ?? 0} tokens!");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to look up coins for {userToCheck}.");
            }
        }
    }
}
