using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public class TaxCommand : BaseCommand
    {
        private readonly IChatUserCollection _chatUserCollection;

        public TaxCommand(IRepository repository, IChatUserCollection chatUserCollection)
            : base(repository)
        {
            _chatUserCollection = chatUserCollection;
        }

        protected override bool HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            string taxedUser = eventArgs?.Arguments?.ElementAtOrDefault(0)?.NoAt();
            string coinsToTakeString = eventArgs?.Arguments?.ElementAtOrDefault(1);

            if (string.IsNullOrWhiteSpace(taxedUser) || string.IsNullOrWhiteSpace(coinsToTakeString))
            {
                chatClient.SendMessage(HelpText);
                return;
            }

            if (int.TryParse(coinsToTakeString, out int coinsToTake))
            {
                if (_chatUserCollection.ReduceCoins(taxedUser, coinsToTake))
                {
                    chatClient.SendMessage($"Our taxation department says you owe, {coinsToTake} coins, {taxedUser}. Thanks!");
                }
                else
                {
                    chatClient.SendMessage($"{taxedUser} failed to pay the {coinsToTake} coins.");
                }

            }
            else
            {
                chatClient.SendMessage($"Is that even a number, {eventArgs?.ChatUser?.DisplayName}?");
            }
        }
    }
}
