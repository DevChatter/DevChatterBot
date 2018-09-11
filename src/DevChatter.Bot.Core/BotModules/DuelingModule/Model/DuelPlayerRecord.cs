using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Core.BotModules.DuelingModule.Model
{
    public class DuelPlayerRecord : DataEntity
    {
        public DuelPlayed Duel { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public string WinLossTie { get; set; }
    }
}
