using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Core.Games.Slots
{
    public class SlotsCommand : BaseCommand
    {
        private readonly ICurrencyGenerator _currencyGenerator;
        private readonly SlotsSettings _slotsSettings;

        private static readonly List<SlotEmote> _emotes = new List<SlotEmote>
        {
            new SlotEmote("Kappa", -5, -1, 0),
            new SlotEmote("PogChamp", 1000, 100, 5),
            new SlotEmote("HeyGuys", 750, 50, 2),
            new SlotEmote("SeemsGood", 100, 20, 1),
            new SlotEmote("NotLikeThis", 20, 5, 0),
            new SlotEmote("BrainSlug", 50, 10, 0),
            new SlotEmote("WutFace", 20, 5, 0),
            new SlotEmote("DarkMode", -50, -10, 0),
            new SlotEmote("SeriousSloth", 500, 50, 5),
        };

        public SlotsCommand(IRepository repository, ICurrencyGenerator currencyGenerator, ISettingsFactory settingsFactory)
            : base(repository)
        {
            _currencyGenerator = currencyGenerator;
            _slotsSettings = settingsFactory.GetSettings<SlotsSettings>();
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            var name = eventArgs.ChatUser.DisplayName;

            if (_currencyGenerator.RemoveCurrencyFrom(name, _slotsSettings.DefaultBet) == 0)
            {
                chatClient.SendMessage($"Sorry, {name}. You need {_slotsSettings.DefaultBet} coins to play.");
                return;
            }

            List<SlotEmote> results = new List<SlotEmote>();

            for (int i = 0; i < 3; i++)
            {
                results.Add(MyRandom.ChooseRandomItem(_emotes).ChosenItem);
            }

            int payoutTotal = CalculatePayout(results);

            AdjustCurrency(payoutTotal, name);

            string response = GetResponse(payoutTotal, name);

            string reelDisplay = string.Join(" ", results.Select(x => x.Text));

            chatClient.SendMessage($"{reelDisplay} - {response}");
        }

        private void AdjustCurrency(int payoutTotal, string name)
        {
            if (payoutTotal > 0)
            {
                _currencyGenerator.AddCurrencyTo(name, payoutTotal);
            }
            else if (payoutTotal < 0)
            {
                _currencyGenerator.RemoveCurrencyFrom(name, Math.Abs(payoutTotal));
            }
        }

        private string GetResponse(int payoutTotal, string name)
        {
            if (payoutTotal > _slotsSettings.DefaultBet)
            {
                return $"Congratulations, {name}! You won {payoutTotal} coins!";
            }

            if (payoutTotal > 0)
            {
                return $"At least you got {payoutTotal} coins back, {name}.";
            }

            if (payoutTotal == 0)
            {
                return $"Better luck next time, {name}.";
            }

            return $"The slots have stolen {Math.Abs(payoutTotal)} from you, {name}.";
        }

        public int CalculatePayout(List<SlotEmote> results)
        {
            int total = 0;
            var emoteGroups = results.GroupBy(x => x.Text);
            foreach (var emoteGroup in emoteGroups)
            {
                switch (emoteGroup.Count())
                {
                    case 1:
                        total += emoteGroup.First().SinglePayout;
                        break;
                    case 2:
                        total += emoteGroup.First().DoublePayout;
                        break;
                    case 3:
                        total += emoteGroup.First().TriplePayout;
                        break;
                }
            }
            return total;
        }
    }
}
