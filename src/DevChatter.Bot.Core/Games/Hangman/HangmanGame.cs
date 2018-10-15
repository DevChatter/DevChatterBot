using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Systems.Streaming;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Core.Games.Hangman
{
    public class HangmanGame : IGame
    {
        private readonly string ALL_LETTERS;

        private readonly List<HangmanGuess> _guessedLetters = new List<HangmanGuess>();

        private string Password;

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

        public string MaskedPasswordForDisplay => string.Join(" ", MaskedPassword.Replace('*', '_').ToArray());

        private string AvailableLetters => string.Join(" ", ALL_LETTERS.Select(x =>
                _guessedLetters.Any(l => l.Letter.EqualsIns(x.ToString())) ? '_': x));

        private int LivesRemaining => 6 - _guessedLetters.Count(x => !Password.Contains(x.Letter));

        private readonly ICurrencyGenerator _currencyGenerator;
        private readonly IRepository _repository;
        private readonly IHangmanDisplayNotification _hangmanDisplayNotification;
        private HangmanSettings _hangmanSettings;

        public bool IsRunning { get; private set; }

        public HangmanGame(ICurrencyGenerator currencyGenerator, IRepository repository, ISettingsFactory settingsFactory, IHangmanDisplayNotification hangmanDisplayNotification)
        {
            _currencyGenerator = currencyGenerator;
            _repository = repository;
            _hangmanDisplayNotification = hangmanDisplayNotification;
            _hangmanSettings = settingsFactory.GetSettings<HangmanSettings>();
            ALL_LETTERS = _hangmanSettings?.AllowedCharacters.ToLowerInvariant();

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
                $"Congratulations, {chatUser.DisplayName} ! You won the game and will get {_hangmanSettings.TokensToWinner} tokens!");
            _currencyGenerator.AddCurrencyTo(new List<string> { chatUser.DisplayName }, _hangmanSettings.TokensToWinner);
            GivePerLetterTokens(chatClient);
            _hangmanDisplayNotification.HangmanWin();
            ResetGame();
        }

        private void GivePerLetterTokens(IChatClient chatClient)
        {
            chatClient.SendMessage($"{_hangmanSettings.TokensPerLetter} tokens will be given for each correctly guessed letter.");
            var tokensToGiveOut = CalculateLetterAwards(_guessedLetters, Password);
            _currencyGenerator.AddCurrencyTo(tokensToGiveOut, _hangmanSettings.TokensPerLetter);
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
            Password = null;
        }

        public void AskAboutLetter(IChatClient chatClient,
            string letterToAsk, ChatUser chatUser)
        {
            letterToAsk = letterToAsk.ToLowerInvariant();
            if (!IsRunning)
            {
                SendGameNotStartedMessage(chatClient, chatUser);
                return;
            }

            if (!ALL_LETTERS.Contains(letterToAsk))
            {
                chatClient.SendMessage($"\"{letterToAsk}\" is not a valid letter");
                return;
            }

            if (_guessedLetters.Any(g => g.Letter == letterToAsk))
            {
                chatClient.SendMessage($"{letterToAsk} has already been guessed, {chatUser.DisplayName}.");
                return;
            }

            _guessedLetters.Add(new HangmanGuess(letterToAsk, chatUser));
            SendAllGuessedLetters();

            if (!Password.Contains(letterToAsk))
            {
                chatClient.SendMessage($"No, {letterToAsk} is not in the word.");
                _hangmanDisplayNotification.HangmanWrongAnswer();
                CheckForGameLost(chatClient);
                return;
            }

            if (Password == MaskedPassword)
            {
                GuessWord(chatClient, MaskedPassword, chatUser);
                return;
            }

            chatClient.SendMessage($"Yep, {letterToAsk} is in here. {MaskedPassword}");
        }

        private void CheckForGameLost(IChatClient chatClient)
        {
            if (LivesRemaining <= 0)
            {
                chatClient.SendMessage(
                    $"That's too many failed guesses. You all lost. devchaFail. The word was: {Password}");
                ResetGame();
                _hangmanDisplayNotification.HangmanLose();
            }
        }

        public void AttemptToStartGame(IChatClient chatClient, ChatUser chatUser)
        {
            if (IsRunning)
            {
                SendGameAlreadyStartedMessage(chatClient, chatUser);
                return;
            }

            if (!chatUser.IsInThisRoleOrHigher(_hangmanSettings.RoleRequiredToStartGame))
            {
                chatClient.SendMessage(
                    $"You must be at least a {_hangmanSettings.RoleRequiredToStartGame} to start a game, {chatUser.DisplayName}");
                return;
            }

            if (ALL_LETTERS == null)
            {
                chatClient.SendMessage("Couldn't start a new game, there are no allowed letters");
                return;
            }

            var wordList = _repository.List<HangmanWord>().OrderBy(x => Guid.NewGuid());
            foreach (var w in wordList)
            {
                char[] letters = w.Word.ToLowerInvariant().ToCharArray();
                if (letters.All(l => ALL_LETTERS.ToLowerInvariant().Contains(l)))
                {
                    Password = w.Word.ToLowerInvariant();
                    break;
                }
            }

            if (Password == null)
            {
                chatClient.SendMessage(
                    "No words in the Hangman dictionary are compatible with the supported words");
            }
            else
            {
                _hangmanDisplayNotification.HangmanStart(AvailableLetters, LivesRemaining, MaskedPasswordForDisplay);
                chatClient.SendMessage($"Totally starting this game. Your word to guess is {MaskedPassword}");
                IsRunning = true;
            }
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

        private void SendAllGuessedLetters()
        {
            _hangmanDisplayNotification.HangmanShowGuessedLetters(AvailableLetters, LivesRemaining, MaskedPasswordForDisplay);
        }
    }
}
