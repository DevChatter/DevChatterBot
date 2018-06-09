using DevChatter.Bot.Core.Games.Roulette;

namespace DevChatter.Bot.Core.Data
{
    public interface ISettingsFactory
    {
        RouletteSettings GetRouletteSettings();
    }
}
