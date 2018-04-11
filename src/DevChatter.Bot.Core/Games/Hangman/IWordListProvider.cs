using System.Collections.Generic;

namespace DevChatter.Bot.Core.Games.Hangman
{
    public interface IWordListProvider
    {
        IList<string> Words { get; }
    }
}