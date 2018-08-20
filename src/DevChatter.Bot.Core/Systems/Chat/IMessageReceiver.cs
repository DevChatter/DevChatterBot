using System;
using DevChatter.Bot.Core.Events.Args;

namespace DevChatter.Bot.Core.Systems.Chat
{
    public interface IMessageReceiver
    {
        event EventHandler<MessageReceivedEventArgs> OnMessageReceived;
        event EventHandler<CommandReceivedEventArgs> OnCommandReceived;
    }
}
