using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Games.Hangman;
using Xunit;

namespace UnitTests.Core.Games.Hangman.HangmanGameTests
{
    public class CalculateLetterAwardsShould
    {
        [Fact]
        public void ReturnRepeatUser_GivenOneGuesser()
        {
            var user1 = new ChatUser {DisplayName = "1"};
            var user2 = new ChatUser {DisplayName = "2"};
            var guessedLetters = new List<HangmanGuess>
            {
                new HangmanGuess("p", user1),
                new HangmanGuess("a", user1),
                new HangmanGuess("s", user1),
                new HangmanGuess("w", user1),
                new HangmanGuess("o", user1),
                new HangmanGuess("r", user1),
                new HangmanGuess("d", user1),
            };
            List<string> awards = HangmanGame.CalculateLetterAwards(guessedLetters, "password");

            Assert.Equal(8, awards.Count(a => a == user1.DisplayName));
            Assert.Equal(0, awards.Count(a => a == user2.DisplayName));
        }

        [Fact]
        public void ReturnCorrectUser_GivenMultipleGuessers()
        {
            var user1 = new ChatUser {DisplayName = "1"};
            var user2 = new ChatUser {DisplayName = "2"};
            var guessedLetters = new List<HangmanGuess>
            {
                new HangmanGuess("p", user1),
                new HangmanGuess("a", user2),
                new HangmanGuess("s", user2),
                new HangmanGuess("w", user2),
                new HangmanGuess("o", user1),
                new HangmanGuess("r", user1),
                new HangmanGuess("d", user1),
            };
            List<string> awards = HangmanGame.CalculateLetterAwards(guessedLetters, "password");

            Assert.Equal(4, awards.Count(a => a == user1.DisplayName));
            Assert.Equal(4, awards.Count(a => a == user2.DisplayName));
        }

        [Fact]
        public void ReturnFewerGuesses_GivenGuessedEarly()
        {
            var user1 = new ChatUser {DisplayName = "1"};
            var user2 = new ChatUser {DisplayName = "2"};
            var guessedLetters = new List<HangmanGuess>
            {
                new HangmanGuess("p", user1),
                new HangmanGuess("a", user2),
                new HangmanGuess("s", user2),
            };
            List<string> awards = HangmanGame.CalculateLetterAwards(guessedLetters, "password");

            Assert.Equal(1, awards.Count(a => a == user1.DisplayName));
            Assert.Equal(3, awards.Count(a => a == user2.DisplayName));
        }
    }
}