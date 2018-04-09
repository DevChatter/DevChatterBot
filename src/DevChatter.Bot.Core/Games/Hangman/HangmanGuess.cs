using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Core.Games.Hangman
{
    public class HangmanGuess
    {
        public HangmanGuess(string letter, ChatUser guesser)
        {
            Letter = letter;
            Guesser = guesser;
        }
        public string Letter { get; }
        public ChatUser Guesser { get; }
    }
}