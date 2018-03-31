using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Games.Hangman
{
    public class HangmanGame
    {
        private const int GUESS_WAIT_IN_SECONDS = 30;
        private const UserRole ROLE_REQUIRE_TO_START = UserRole.Subscriber;
        private const int TOKENS_TO_WINNER = 25;

        private readonly List<string> _guessedLetters = new List<string>();

        private readonly List<string> _wordList;

        private string _password;
        public string Password => _password ?? (_password = _wordList.OrderBy(x => Guid.NewGuid()).FirstOrDefault());

        public string MaskedPassword
        {
            get { return new string(Password.Select(x => _guessedLetters.Contains(x.ToString()) ? x : '*').ToArray()); }
        }

        private readonly CurrencyGenerator _currencyGenerator;
        private readonly AutomatedActionSystem _automatedActionSystem;

        private bool _isRunningGame;

        public HangmanGame(CurrencyGenerator currencyGenerator, AutomatedActionSystem automatedActionSystem, 
            List<string> wordList)
        {
            _currencyGenerator = currencyGenerator;
            _automatedActionSystem = automatedActionSystem;
            _wordList = wordList;
        }

        public void GuessWord(IChatClient chatClient, string guess, ChatUser chatUser)
        {
            if (!_isRunningGame)
            {
                SendGameNotStartedMessage(chatClient, chatUser);
                return;
            }

            if (guess.Equals(Password, StringComparison.InvariantCultureIgnoreCase))
            {
                ResetGame();
                chatClient.SendMessage($"Congratulations, {chatUser.DisplayName} ! You won the game!");
                _currencyGenerator.AddCurrencyTo(new List<string>{chatUser.DisplayName}, TOKENS_TO_WINNER);
            }
            else
            {
                chatClient.SendMessage($"That is not the word, {chatUser.DisplayName} ...");
            }
        }

        private void ResetGame()
        {
            _isRunningGame = false;
            _guessedLetters.Clear();
            _password = null;
        }

        public void AskAboutLetter(IChatClient chatClient, string letterToAsk, ChatUser chatUser)
        {
            if (!_isRunningGame)
            {
                SendGameNotStartedMessage(chatClient, chatUser);
                return;
            }

            if (_guessedLetters.Contains(letterToAsk))
            {
                chatClient.SendMessage($"{letterToAsk} has already been guessed, {chatUser.DisplayName}.");
                return;
            }

            _guessedLetters.Add(letterToAsk);
            if (Password.Contains(letterToAsk))
            {
                chatClient.SendMessage($"Yep, {letterToAsk} is in here. {MaskedPassword}");
            }
            else
            {
                chatClient.SendMessage($"No, {letterToAsk} is not in the word.");
            }
        }

        public void AttemptToStartGame(IChatClient chatClient, ChatUser chatUser)
        {
            if (_isRunningGame)
            {
                SendGameAlreadyStartedMessage(chatClient, chatUser);
                return;
            }

            if (!chatUser.CanUserRunCommand(ROLE_REQUIRE_TO_START))
            {
                chatClient.SendMessage($"You must be at least a {ROLE_REQUIRE_TO_START} to start a game, {chatUser.DisplayName}");
                return;
            }

            _isRunningGame = true;

            chatClient.SendMessage($"Totally starting this game. You word to guess is {MaskedPassword}");
        }

        private void SendGameAlreadyStartedMessage(IChatClient chatClient, ChatUser chatUser)
        {
            chatClient.SendMessage($"There is already a {nameof(HangmanGame)} running, {chatUser.DisplayName}. Use !Hangman \"letter\" to guess a letter or !Hangman \"word\" to guess the word!");
        }

        private void SendGameNotStartedMessage(IChatClient chatClient, ChatUser chatUser)
        {
            chatClient.SendMessage($"There's no {nameof(HangmanGame)} running, {chatUser.DisplayName}.");
        }
    }
}