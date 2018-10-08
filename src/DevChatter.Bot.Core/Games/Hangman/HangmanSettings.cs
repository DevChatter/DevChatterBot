using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Core.Games.Hangman
{
    public class HangmanSettings
    {
        public UserRole RoleRequiredToDeleteWord { get; set; } = UserRole.Mod;
        public UserRole RoleRequiredToStartGame { get; set; } = UserRole.Subscriber;

    }
}
