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

        private readonly List<string> _guessedLetters = new List<string>();

        private const string PASSWORD = "password";

        public string MaskedPassword
        {
            get { return new string(PASSWORD.Select(x => _guessedLetters.Contains(x.ToString()) ? x : '*').ToArray()); }
        }

        private readonly CurrencyGenerator _currencyGenerator;
        private readonly AutomatedActionSystem _automatedActionSystem;

        private bool _isRunningGame;

        public HangmanGame(CurrencyGenerator currencyGenerator, AutomatedActionSystem automatedActionSystem)
        {
            _currencyGenerator = currencyGenerator;
            _automatedActionSystem = automatedActionSystem;
        }

        public void GuessWord(IChatClient chatClient, string guess, ChatUser chatUser)
        {
            if (!_isRunningGame)
            {
                SendGameNotStartedMessage(chatClient, chatUser);
                return;
            }

            if (guess.Equals(PASSWORD, StringComparison.InvariantCultureIgnoreCase))
            {
                ResetGame();
                chatClient.SendMessage($"congratulations, {chatUser.DisplayName} ! You won this pointless game! No points for you!");
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
            if (PASSWORD.Contains(letterToAsk))
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