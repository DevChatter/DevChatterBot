using DevChatter.Bot.Core.Commands;

namespace DevChatter.Bot.Core.Events
{
    public abstract class Cooldown
    {
        public string Message { get; set; }
    }

    public class NoCooldown : Cooldown
    {
    }

    /// <summary>
    /// This cooldown indicates that a user has too recently run any command.
    /// </summary>
    public class UserCooldown : Cooldown
    {
    }

    /// <summary>
    /// This cooldown indicates that a command has been run too recently by anyone.
    /// </summary>
    public class CommandCooldown : Cooldown
    {
        public IBotCommand Command { get; set; }
    }

    /// <summary>
    /// This cooldown indicates that this command has been run too recently by this user.
    /// </summary>
    public class UserCommandCooldown : Cooldown
    {
        public IBotCommand Command { get; set; }
    }
}
