using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Model;

namespace DevChatter.Bot.Core.Messaging
{
    public interface IBotCommand
    {
        UserRole RoleRequired { get; }
        string CommandText { get; }
        void Process(IChatClient triggeringClient, CommandReceivedEventArgs eventArgs);

    }
}