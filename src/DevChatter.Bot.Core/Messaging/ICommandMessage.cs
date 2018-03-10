using DevChatter.Bot.Core.Events;

namespace DevChatter.Bot.Core.Messaging
{
    public interface ICommandMessage
    {
        string CommandText { get; }
        void Process(IChatClient triggeringClient, CommandReceivedEventArgs eventArgs);

    }
}