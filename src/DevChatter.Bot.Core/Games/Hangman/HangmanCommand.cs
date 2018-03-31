using System.Linq;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Games.Hangman
{
    public class HangmanCommand : SimpleCommand
    {
        private readonly HangmanGame _hangmanGame;

        public HangmanCommand(HangmanGame hangmanGame)
        {
            _hangmanGame = hangmanGame;
            CommandText = "Hangman";
            RoleRequired = UserRole.Everyone;
        }

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            string argumentOne = eventArgs?.Arguments?.FirstOrDefault();
            ChatUser chatUser = eventArgs?.ChatUser;
            if (string.IsNullOrWhiteSpace(argumentOne))
            {
                // attempting to start a game
                _hangmanGame.AttemptToStartGame(chatClient, chatUser);
            }
            else if (argumentOne.Length == 1)
            {
                // asking about a letter
                _hangmanGame.AskAboutLetter(chatClient, argumentOne.ToLowerInvariant(), chatUser);
            }
            else
            {
                // guessing the word
                _hangmanGame.GuessWord(chatClient, argumentOne, chatUser);
            }
        }
    }
}