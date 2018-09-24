using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Systems.Chat;
using System.Linq;

namespace DevChatter.Bot.Core.Commands
{
    public class BonusCommand : BaseCommand
    {
        private readonly ICurrencyGenerator _currencyGenerator;

        public BonusCommand(IRepository repository, ICurrencyGenerator currencyGenerator)
            : base(repository)
        {
            _currencyGenerator = currencyGenerator;
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            string bonusReceiver = (eventArgs?.Arguments?.ElementAtOrDefault(0) ?? "").NoAt();
            string bonusGiver = eventArgs?.ChatUser?.DisplayName;
            if (bonusGiver.EqualsIns(bonusReceiver))
            {
                chatClient.SendMessage($"Did you seriously think this would work, {bonusGiver}");
                return;
            }

            if (int.TryParse(eventArgs?.Arguments?.ElementAtOrDefault(1), out int amount))
            {
                if (amount < 0)
                {
                    chatClient.SendMessage($"Quit messing around, {bonusGiver}. You can't give negative amounts.");
                    return;
                }

                _currencyGenerator.AddCurrencyTo(bonusReceiver, amount); // TODO: prevent overflow
                chatClient.SendMessage($"Added {amount} coins to @{bonusReceiver}.");
            }
        }
    }
}
