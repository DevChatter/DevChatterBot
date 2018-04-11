using System.Collections.Generic;

namespace DevChatter.Bot.Core.Games.Hangman
{
    public class HardcodedWordListProvider : IWordListProvider
    {
        public IList<string> Words { get; } = new[]
        {
            "apple", "banana", "orange", "mango", "watermellon", "grapes", "pizza", "pasta",
            "pepperoni", "cheese", "mushroom", "csharp", "javascript", "cplusplus",
            "nullreferenceexception", "parameter", "argument"
        };
    }
}
