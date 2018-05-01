using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using System.Linq;

namespace DevChatter.Bot.Core.Games.RockPaperScissors
{
    public class RockPaperScissorsCommand : BaseCommand, IGameCommand
    {
        private readonly RockPaperScissorsGame _rockPaperScissorsGame;

        public IGame Game => _rockPaperScissorsGame;

        public RockPaperScissorsCommand(IRepository repository, RockPaperScissorsGame rockPaperScissorsGame)
            : base(repository, UserRole.Everyone)
        {
            _rockPaperScissorsGame = rockPaperScissorsGame;
            HelpText =
                "Use \"!rps\" to join randomly. Use \"!rps rock\" to select rock. Bot will eventually choose randomly and award the winners.";
        }

        public override string FullHelpText => "Use \"!rps\" to start or join the game randomly. To start or join with a selection, you can use \"!rps rock\", \"!rps paper\", \"!rps scissors\", to select rock, paper, or scissors respectively. The bot will wait 120 seconds and then randomly choose its own and anyone who beats the bot will be awarded tokens.";

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
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
