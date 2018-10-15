using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Systems.Streaming;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Core.BotModules.VotingModule
{
    public class VotingSystem
    {
        private readonly IVotingDisplayNotification _votingDisplayNotification;

        private readonly Dictionary<string, int> _votes
            = new Dictionary<string, int>();
        private readonly Dictionary<int, string> _choices
            = new Dictionary<int, string>();

        public bool IsVoteActive { get; set; }

        public VotingSystem(IVotingDisplayNotification votingDisplayNotification)
        {
            _votingDisplayNotification = votingDisplayNotification;
        }

        public void ApplyVote(ChatUser chatUser, string choice, IChatClient chatClient)
        {
            if (!IsVoteActive) { return; }

            bool isValidNumber = int.TryParse(choice, out int chosenNumber)
                && _choices.ContainsKey(chosenNumber);

            string voteText = isValidNumber ? _choices[chosenNumber] : "nothing";

            _votes[chatUser.UserId] = chosenNumber;

            if (isValidNumber)
            {
                int[] voteTotals = _choices.Select(c => _votes.Count(x => x.Value == c.Key)).ToArray();
                _votingDisplayNotification.VoteReceived(chatUser, voteText, voteTotals);
            }

            string message = $"{chatUser.DisplayName} voted for {voteText}.";
            chatClient.SendMessage(message);
        }

        public string StartVote(List<string> newVoteArguments)
        {
            for (int i = 0; i < newVoteArguments.Count; i++)
            {
                _choices[i + 1] = newVoteArguments[i];
            }
            IsVoteActive = true;

            string optionsString = string.Join(", ", _choices.OrderBy(x => x.Key).Select(x => $"({x.Key}) {x.Value}"));

            _votingDisplayNotification.VoteStart(_choices.Select(x => x.Value));

            return $"Voting has started. To vote type \"!vote [number]\" (ex: !vote 2). Options: {optionsString}";
        }

        public string EndVoting()
        {
            string resultMessage = GetResultsOfVote();
            ResetVote();
            _votingDisplayNotification.VoteEnd();
            return resultMessage;
        }

        private string GetResultsOfVote()
        {
            List<VoteCount> choiceVotes = GetVoteCounts();
            if (!choiceVotes.Any())
            {
                return VotingMessages.NO_WINNER;
            }
            int topVoteCount = choiceVotes.Max(ch => ch.Votes);
            var topChoices = choiceVotes.Where(ch => ch.Votes == topVoteCount).ToList();
            if (topChoices.Count > 1)
            {
                string winnersString = string.Join(", ", topChoices.Select(c => _choices[c.ChoiceKey]));
                return $"There's a tie: ({winnersString}) with vote count: {topVoteCount}!";
            }

            if (topChoices.Count == 1)
            {
                return $"{_choices[topChoices.Single().ChoiceKey]} wins! Vote count: {topVoteCount}!";
            }

            return VotingMessages.NO_WINNER;
        }

        private List<VoteCount> GetVoteCounts()
        {
            List<VoteCount> choiceVotes = _votes
                .Where(x => x.Value > 0)
                .GroupBy(x => x.Value)
                .Select(grp => new VoteCount {ChoiceKey = grp.Key, Votes = grp.Count()})
                .ToList();
            return choiceVotes;
        }

        private void ResetVote()
        {
            _votes.Clear();
            _choices.Clear();
            IsVoteActive = false;
        }
    }

    public static class VotingMessages
    {
        public const string NO_WINNER = "Everyone wins, because you're all awesome!";
    }

    internal class VoteCount
    {
        public int ChoiceKey { get; set; }
        public int Votes { get; set; }
    }
}
