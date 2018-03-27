using System.Linq;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Games.RockPaperScissors
{
    public class RockPaperScissorsCommand : SimpleCommand
    {
        private readonly RockPaperScissorsGame _rockPaperScissorsGame;
        public RockPaperScissorsCommand(RockPaperScissorsGame rockPaperScissorsGame)
        {
            _rockPaperScissorsGame = rockPaperScissorsGame;
            CommandText = "rps";
            RoleRequired = UserRole.Everyone;
        }

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            string username = eventArgs?.ChatUser?.DisplayName;
            string argumentOne = eventArgs?.Arguments?.ElementAtOrDefault(0);
            if (_rockPaperScissorsGame.IsReadyForNewMatch())
            {
                _rockPaperScissorsGame.AttemptToStartNewGame(chatClient, username);
            }

            if (!RockPaperScissors.GetByName(argumentOne, out RockPaperScissors choice))
            {
                choice = RockPaperScissors.GetRandomChoice();
                chatClient.SendMessage($"{username} didn't want to pick, so we randomly assigned {choice}!");
            }
            _rockPaperScissorsGame.JoinMatch(chatClient, (username, choice));
        }
    }
}