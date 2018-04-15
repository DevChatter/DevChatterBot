using DevChatter.Bot.Core.Commands;

namespace DevChatter.Bot.Core.Games
{
    public interface IGameCommand : IBotCommand
    {
        IGame Game { get; }
    }
}
