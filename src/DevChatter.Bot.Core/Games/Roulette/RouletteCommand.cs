using System;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Games.Roulette
{
    public class RouletteCommand : BaseCommand
    {
        private readonly ICurrencyGenerator _currencyGenerator;
        private readonly RouletteSettings _rouletteSettings;

        public RouletteCommand(IRepository repository, ICurrencyGenerator currencyGenerator, ISettingsFactory settingsFactory)
            : base(repository, UserRole.Everyone)
        {
            _currencyGenerator = currencyGenerator;
            _rouletteSettings = settingsFactory.GetRouletteSettings();
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            var name = eventArgs.ChatUser.DisplayName;
            var random = MyRandom.RandomNumber(0, 100);

            if (random < _rouletteSettings.WinPercentageChance)
            {
                chatClient.SendMessage($"{name} pulls the trigger and the revolver clicks! {name} survived the roulette!");
                _currencyGenerator.AddCurrencyTo(name, _rouletteSettings.CoinsReward);
            }
            else
            {
                if (_rouletteSettings.ProtectSubscribers && eventArgs.ChatUser.Role == UserRole.Subscriber)
                {
                    chatClient.SendMessage(
                        $"{name} pulls the trigger and the revolver fires! A bright light flashes through chat and revives {name}");
                }
                else
                {
                    chatClient.SendMessage($"{name} pulls the trigger and the revolver fires! {name} lies dead in chat. RIP");
                    chatClient.Timeout(name, TimeSpan.FromSeconds(_rouletteSettings.TimeoutDurationInSeconds), "Died playing roulette");
                }
            }
        }
    }
}
