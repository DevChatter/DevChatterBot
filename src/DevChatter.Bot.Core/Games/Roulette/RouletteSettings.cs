namespace DevChatter.Bot.Core.Games.Roulette
{
    public class RouletteSettings
    {
        public int WinPercentageChance { get; set; } = 20;
        public int TimeoutDurationInSeconds { get; set; } = 10;
        public bool ProtectSubscribers { get; set; } = true;
        public int CoinsReward { get; set; } = 100;
    }
}
