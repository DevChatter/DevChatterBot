using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public class GiveCommand : BaseCommand
    {
        private readonly IChatUserCollection _chatUserCollection;

        public GiveCommand(IRepository repository, IChatUserCollection chatUserCollection)
            : base(repository, UserRole.Everyone)
        {
            _chatUserCollection = chatUserCollection;
            HelpText = "Give coins to someone. Example !give LNGgrinds 10";
        }

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            string coinGiver = eventArgs?.ChatUser?.DisplayName;
            string coinReceiver = eventArgs?.Arguments?.ElementAtOrDefault(0).NoAt();
            string coinsToGiveText = eventArgs?.Arguments?.ElementAtOrDefault(1);

            if (string.IsNullOrWhiteSpace(coinReceiver) || string.IsNullOrWhiteSpace(coinsToGiveText) || string.IsNullOrWhiteSpace(coinGiver))
            {
                chatClient.SendMessage(HelpText);
                return;
            }

            if (int.TryParse(coinsToGiveText, out int coinsToGive))
            {
                if (coinsToGive < 2)
                {
                    chatClient.SendMessage("Cheapskate. You should consider giving more coins...");
                    return;
                }

                if (_chatUserCollection.TryGiveCoins(coinGiver, coinReceiver, coinsToGive))
                {
                    chatClient.SendMessage($"Taking {coinsToGive} coins from {coinGiver} and giving them to {coinReceiver}.");
                }
                else
                {
                    chatClient.SendMessage("Failed to send coins.");
                }
            }
            else
            {
                chatClient.SendMessage($"Is that even a number, {coinGiver}?");
            }

        }
    }
}