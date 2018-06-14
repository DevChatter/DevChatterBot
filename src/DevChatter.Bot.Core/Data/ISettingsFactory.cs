using DevChatter.Bot.Core.Games.Roulette;

namespace DevChatter.Bot.Core.Data
{
    public interface ISettingsFactory
    {
        T GetSettings<T>() where T : class, new();

    }
}
