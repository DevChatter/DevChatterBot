using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Commands.Operations;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Games.Hangman
{
    public class HangmanCommand : BaseCommand, IGameCommand
    {
        private List<ICommandOperation> _operations;

        public List<ICommandOperation> Operations => _operations ?? (_operations = new List<ICommandOperation>
        {
            new AddHangmanWordOperation(Repository),
            new GenericDeleteOperation<HangmanWord>(Repository, _hangmanSettings.RoleRequiredToDeleteWord,
                e => HangmanWordPolicy.ByWord(e.Arguments?.ElementAtOrDefault(1))),
        });

        private static readonly object SingleFileLock = new object();

        private readonly HangmanGame _hangmanGame;
        private readonly HangmanSettings _hangmanSettings;
        public IGame Game => _hangmanGame;

        public HangmanCommand(IRepository repository, ISettingsFactory settingsFactory, HangmanGame hangmanGame)
            : base(repository)
        {
            _hangmanSettings = settingsFactory.GetSettings<HangmanSettings>();
            _hangmanGame = hangmanGame;
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            string argumentOne = eventArgs?.Arguments?.FirstOrDefault();
            ChatUser chatUser = eventArgs?.ChatUser;

            var operationToUse = Operations.SingleOrDefault(x => x.ShouldExecute(argumentOne));
            if (operationToUse != null)
            {
                string resultMessage = operationToUse.TryToExecute(eventArgs);
                chatClient.SendMessage(resultMessage);
            }
            else
            {
                HandleGamePlayingRequests(argumentOne, chatUser, chatClient);
            }
        }

        public void HandleGamePlayingRequests(string argumentOne, ChatUser chatUser, IChatClient chatClient)
        {
            lock (SingleFileLock)
            {
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
}
