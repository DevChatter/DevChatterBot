using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Events.Args;
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
            _chatClient.OnWhisperReceived += ChatClientOnOnWhisperReceived;
        }

        private void ChatClientOnOnWhisperReceived(object sender, WhisperReceivedEventArgs e)
        {
            if (!_ongoingDuels.Any()) { return; }
            Duel existingDuel = _ongoingDuels
                .SingleOrDefault(duel => duel.IsExpectingInputFrom(e.FromDisplayName));
            DuelResult duelResult = existingDuel?.ApplySelection(e.FromDisplayName, e.Message);

            if (duelResult.DuelIsOver)
            {
                _ongoingDuels.Remove(existingDuel);
            }

            if (!string.IsNullOrWhiteSpace(duelResult.MessageForUser))
            {
                _chatClient.SendDirectMessage(e.FromDisplayName, duelResult.MessageForUser);
            }

            if (!string.IsNullOrWhiteSpace(duelResult.MessageForChat))
            {
                _chatClient.SendMessage(duelResult.MessageForChat);
            }
        }

        private readonly List<Duel> _ongoingDuels = new List<Duel>();

        public Duel GetChallenges(string challenger, string opponent)
        {
            return _ongoingDuels
                .SingleOrDefault(x => x.Opponent == challenger && x.Challenger == opponent);
        }

        public bool RequestDuel(string challenger, string opponent)
        {
            if (IsUserInAnotherDuel(challenger))
            {
                _chatClient.SendMessage($"Sorry, {challenger}, you are already in another duel.");
                return false;
            }

            if (IsUserInAnotherDuel(opponent))
            {
                _chatClient.SendMessage($"Sorry, {opponent} is in another duel already.");
                return false;
            }

            _ongoingDuels.Add(new Duel { Challenger = challenger, Opponent = opponent.NoAt() });
            return true;
        }

        private bool IsUserInAnotherDuel(string userDisplayName)
        {
            return _ongoingDuels.Any(x => x.Challenger.EqualsIns(userDisplayName)
                                          || x.Opponent.EqualsIns(userDisplayName));
        }

        public void Accept(Duel existingChallenge)
        {
            existingChallenge.Start();
            var startMessage = "Choose your weapon by replying to this with: Rock, Paper, Or Scissors.";
            _chatClient.SendDirectMessage(existingChallenge.Challenger, startMessage);
            _chatClient.SendDirectMessage(existingChallenge.Opponent, startMessage);
        }
    }
}
