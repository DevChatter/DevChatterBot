using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Games.Roulette;
using System.Linq;

namespace DevChatter.Bot.Core.Data
{
    public class SettingsFactory : ISettingsFactory
    {
        private readonly IRepository _repository;
        public SettingsFactory(IRepository repository)
        {
            _repository = repository;
        }

        public RouletteSettings GetRouletteSettings()
        {
            var rouletteSettings = new RouletteSettings();

            var settings = _repository.List(CommandSettingsPolicy.ByCommandName(nameof(RouletteSettings)));

            // yes, I'm aware this is ugly, it's going away.....
            var coinsReward = settings.SingleOrDefault(x => x.Key == nameof(rouletteSettings.CoinsReward));
            if (coinsReward != null)
            {
                rouletteSettings.CoinsReward = int.Parse(coinsReward.Value);
            }

            var protectSubscribers = settings.SingleOrDefault(x => x.Key == nameof(rouletteSettings.ProtectSubscribers));
            if (protectSubscribers != null)
            {
                rouletteSettings.ProtectSubscribers = bool.Parse(protectSubscribers.Value);
            }

            var timeoutDuration = settings.SingleOrDefault(x => x.Key == nameof(rouletteSettings.TimeoutDurationInSeconds));
            if (timeoutDuration != null)
            {
                rouletteSettings.TimeoutDurationInSeconds = int.Parse(timeoutDuration.Value);
            }

            var winPercentageChance = settings.SingleOrDefault(x => x.Key == nameof(rouletteSettings.WinPercentageChance));
            if (winPercentageChance != null)
            {
                rouletteSettings.WinPercentageChance = int.Parse(winPercentageChance.Value);
            }

            return rouletteSettings;
        }
    }
}
