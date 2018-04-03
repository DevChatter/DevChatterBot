using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public class BonusCommand : IBotCommand
    {
        private readonly CurrencyGenerator _currencyGenerator;
        public UserRole RoleRequired { get; }
        public string CommandText { get; }
        public string HelpText { get; }

        public BonusCommand(CurrencyGenerator currencyGenerator)
        {
            _currencyGenerator = currencyGenerator;
            CommandText = "Bonus";
            RoleRequired = UserRole.Mod;
            HelpText = "Use the bonus command to give free coins to someone example: !bonus sadukie 50";
        }

        public void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            string bonusReceiver = (eventArgs?.Arguments?.ElementAtOrDefault(0) ?? "").NoAt();
            string bonusGiver = eventArgs?.ChatUser?.DisplayName;
            if (bonusGiver == bonusReceiver)
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
                _currencyGenerator.AddCurrencyTo(new List<string> {bonusReceiver}, amount); // TODO: prevent overflow
                chatClient.SendMessage($"Added {amount} coins to @{bonusReceiver}.");
            }
        }
    }
}