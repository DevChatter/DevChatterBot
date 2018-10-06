namespace DevChatter.Bot.Core.BotModules.VotingModule
{
    public class VoteInfoDto
    {
        public string VoterName { get; set; }
        public string VoterChoiceName { get; set; }
        public int[] VoteTotals { get; set; }
    }
}
