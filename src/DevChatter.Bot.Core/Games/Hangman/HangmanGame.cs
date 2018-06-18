using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Games.Hangman
{
    public class HangmanGame : IGame
    {
        private const int GUESS_WAIT_IN_SECONDS = 30;
        private const UserRole ROLE_REQUIRE_TO_START = UserRole.Subscriber;
        private const int PER_LETTER_TOKENS = 2;
        private const int TOKENS_TO_WINNER = 25;

        private readonly List<HangmanGuess> _guessedLetters = new List<HangmanGuess>();

        private string _password;

        public string Password =>
            _password ?? (_password = _repository.List<HangmanWord>().OrderBy(x => Guid.NewGuid()).FirstOrDefault().Word.ToLowerInvariant());

        public string MaskedPassword
        {
            get
            {
                return new string(
                    Password.Select(x =>
                        {
                            var guessedLetters = _guessedLetters.Select(g => g.Letter);
                            return guessedLetters.Contains(x.ToString()) ? x : '*';
                        })
                        .ToArray()
                );
            }
        }

        private readonly ICurrencyGenerator _currencyGenerator;
        private readonly IAutomatedActionSystem _automatedActionSystem;
        private readonly IRepository _repository;

        public bool IsRunning { get; private set; }

        public HangmanGame(ICurrencyGenerator currencyGenerator, IAutomatedActionSystem automatedActionSystem,
            IRepository repository)
        {
            _currencyGenerator = currencyGenerator;
            _automatedActionSystem = automatedActionSystem;
            _repository = repository;
        }

        public void GuessWord(IChatClient chatClient, string guess, ChatUser chatUser)
        {
            if (!IsRunning)
            {
                SendGameNotStartedMessage(chatClient, chatUser);
                return;
            }

            if (guess.Equals(Password, StringComparison.InvariantCultureIgnoreCase))
            {
                GameWon(chatClient, chatUser);
            }
            else
            {
                chatClient.SendMessage($"That is not the word, {chatUser.DisplayName} ...");
            }
        }

        private void GameWon(IChatClient chatClient, ChatUser chatUser)
        {
            chatClient.SendMessage(
                $"Congratulations, {chatUser.DisplayName} ! You won the game and will get {TOKENS_TO_WINNER} tokens!");
            _currencyGenerator.AddCurrencyTo(new List<string> {chatUser.DisplayName}, TOKENS_TO_WINNER);
            GivePerLetterTokens(chatClient);
            ResetGame();
        }

        private void GivePerLetterTokens(IChatClient chatClient)
        {
            chatClient.SendMessage($"{PER_LETTER_TOKENS} tokens will be given for each correctly guessed letter.");
            var tokensToGiveOut = CalculateLetterAwards(_guessedLetters, Password);
            _currencyGenerator.AddCurrencyTo(tokensToGiveOut, PER_LETTER_TOKENS);
        }

        public static List<string> CalculateLetterAwards(List<HangmanGuess> guessedLetters, string password)
        {
            IEnumerable<HangmanGuess> hangmanGuesses = password.Select(letter =>
                guessedLetters.SingleOrDefault(g => g.Letter == letter.ToString())).Where(x => x != null);

            return hangmanGuesses.Select(x => x.Guesser.DisplayName).ToList();
        }

        private void ResetGame()
        {
            IsRunning = false;
            _guessedLetters.Clear();
            _password = null;
        }

        public void AskAboutLetter(IChatClient chatClient, string letterToAsk, ChatUser chatUser)
        {
            if (!IsRunning)
            {
                SendGameNotStartedMessage(chatClient, chatUser);
                return;
            }

            if (_guessedLetters.Any(g => g.Letter == letterToAsk))
            {
                chatClient.SendMessage($"{letterToAsk} has already been guessed, {chatUser.DisplayName}.");
                return;
            }

            _guessedLetters.Add(new HangmanGuess(letterToAsk, chatUser));
            if (Password.Contains(letterToAsk))
            {
                if (Password == MaskedPassword)
                {
                    GuessWord(chatClient, MaskedPassword, chatUser);
                    return;
                }

                chatClient.SendMessage($"Yep, {letterToAsk} is in here. {MaskedPassword}");
            }
            else
            {
                chatClient.SendMessage($"No, {letterToAsk} is not in the word.");
                CheckForGameLost(chatClient);
            }
        }

        private void CheckForGameLost(IChatClient chatClient)
        {
            if (_guessedLetters.Count(x => !Password.Contains(x.Letter)) > 5)
            {
                chatClient.SendMessage(
                    $"That's too many failed guesses. You all lost. devchaFail. The word was: {Password}");
                ResetGame();
            }
        }

        public void AttemptToStartGame(IChatClient chatClient, ChatUser chatUser)
        {
            if (IsRunning)
            {
                SendGameAlreadyStartedMessage(chatClient, chatUser);
                return;
            }

            if (!chatUser.IsInThisRoleOrHigher(ROLE_REQUIRE_TO_START))
            {
                chatClient.SendMessage(
                    $"You must be at least a {ROLE_REQUIRE_TO_START} to start a game, {chatUser.DisplayName}");
                return;
            }

            IsRunning = true;

            chatClient.SendMessage($"Totally starting this game. You word to guess is {MaskedPassword}");
        }

        private void SendGameAlreadyStartedMessage(IChatClient chatClient, ChatUser chatUser)
        {
            chatClient.SendMessage(
                $"There is already a {nameof(HangmanGame)} running, {chatUser.DisplayName}. Use !Hangman \"letter\" to guess a letter or !Hangman \"word\" to guess the word!");
        }

        private void SendGameNotStartedMessage(IChatClient chatClient, ChatUser chatUser)
        {
            chatClient.SendMessage($"There's no {nameof(HangmanGame)} running, {chatUser.DisplayName}.");
        }
    }
}
