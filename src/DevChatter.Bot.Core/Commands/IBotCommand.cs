using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public interface IBotCommand
    {
        UserRole RoleRequired { get; }
        string CommandText { get; }
        void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs);

    }
}