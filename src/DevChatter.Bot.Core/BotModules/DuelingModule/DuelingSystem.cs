using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.BotModules.DuelingModule
{
    public class DuelingSystem
    {
        private readonly IChatClient _chatClient;

        public DuelingSystem(IChatClient chatClient)
        {
            _chatClient = chatClient;
        }

        private readonly List<Duel> _ongoingDuels = new List<Duel>();

        public Duel GetChallenges(string challenger, string opponent)
        {
            return _ongoingDuels
                .SingleOrDefault(x => x.Opponent == challenger && x.Challenger == opponent);
        }

        public void RequestDuel(string challenger, string opponent)
        {
            _ongoingDuels.Add(new Duel {Challenger = challenger, Opponent = opponent.NoAt() });
        }

        public void Accept(Duel existingChallenge)
        {
            existingChallenge.Start();
            var startMessage = "Choose your weapon: Rock, Paper, Or Scissors.";
            _chatClient.SendDirectMessage(existingChallenge.Challenger, startMessage);
            _chatClient.SendDirectMessage(existingChallenge.Opponent, startMessage);
        }
    }
}
