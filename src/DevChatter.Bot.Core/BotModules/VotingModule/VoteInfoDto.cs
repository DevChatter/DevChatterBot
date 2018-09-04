namespace DevChatter.Bot.Core.BotModules.VotingModule
{
    public class VoteInfoDto
    {
        public string VoterName { get; set; }
        public int VoterChoice { get; set; }
        public int[] VoteTotals { get; set; }
    }
}
