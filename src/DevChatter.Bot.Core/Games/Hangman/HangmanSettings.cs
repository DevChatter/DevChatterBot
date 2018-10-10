using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Core.Games.Hangman
{
    public class HangmanSettings
    {
        public UserRole RoleRequiredToDeleteWord { get; set; } = UserRole.Mod;
        public UserRole RoleRequiredToStartGame { get; set; } = UserRole.Subscriber;
        public int TokensPerLetter { get; set; } = 2;
        public int TokensToWinner { get; set; } = 25;
        public string AllowedCharacters { get; set; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    }
}
