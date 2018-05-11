using System;

namespace DevChatter.Bot.Core.Data.Model
{
    public class HangmanWord : DataEntity
    {
        public HangmanWord()
        {
        }

        public HangmanWord(string word)
        {
            Word = word;
        }

        public string Word { get; protected set; }
    }
}
