using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Systems.Chat;
using System;
using System.Linq;

namespace DevChatter.Bot.Core.Commands
{
    public class CoinsCommand : BaseCommand
    {
        public CoinsCommand(IRepository repository)
            : base(repository, UserRole.Everyone)
        {
            HelpText = "Use \"!coins\" to see your current coin total, or use \"!coins SomeoneElse\" to see another user's coin total.";
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

                ChatUser chatUser = Repository.Single(ChatUserPolicy.ByDisplayName(userToCheck));

                chatClient.SendMessage($"{userToCheck} has {chatUser?.Tokens ?? 0} tokens!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
