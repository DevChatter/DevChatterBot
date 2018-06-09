using System;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Commands
{
    public class RouletteCommand : BaseCommand
    {
        private readonly ICurrencyGenerator _currencyGenerator;

        public RouletteCommand(IRepository repository, ICurrencyGenerator currencyGenerator)
            : base(repository, UserRole.Everyone)
        {
            _currencyGenerator = currencyGenerator;
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            var winPercentageChance = GetSetting<int>("win_percentage_chance", "20");
            var timeoutDurationInSeconds = GetSetting<int>("timeout_duration_in_seconds", "10");
            var protectSubscribers = GetSetting<bool>("protect_subscribers", "true");
            var coinsReward = GetSetting<int>("coins_reward", "100");
            var name = eventArgs.ChatUser.DisplayName;
            var random = MyRandom.RandomNumber(0, 100);

            if (random < winPercentageChance)
            {
                chatClient.SendMessage($"{name} pulls the trigger and the revolver clicks! {name} survived the roulette!");
                _currencyGenerator.AddCurrencyTo(name, coinsReward);
            }
            else
            {
                if (protectSubscribers && eventArgs.ChatUser.Role == UserRole.Subscriber)
                {
                    chatClient.SendMessage(
                        $"{name} pulls the trigger and the revolver fires! A bright light flashes through chat and revives {name}");
                }
                else
                {
                    chatClient.SendMessage($"{name} pulls the trigger and the revolver fires! {name} lies dead in chat. RIP");
                    chatClient.Timeout(name, TimeSpan.FromSeconds(timeoutDurationInSeconds), "Died playing roulette");
                }
            }
        }
    }
}
