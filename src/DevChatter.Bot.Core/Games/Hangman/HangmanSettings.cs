using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Core.Games.Hangman
{
    public class HangmanSettings
    {
        public UserRole DeleteWordRoleRequired { get; set; } = UserRole.Mod;

    }
}
