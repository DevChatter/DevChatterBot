using System;
using System.Threading.Tasks;
using DevChatter.Bot.Core.Events;

namespace DevChatter.Bot.Core.ChatSystems
{
    public interface IChatClient
    {
        Task Connect();

        Task Disconnect();

        void SendMessage(string message);

        event EventHandler<CommandReceivedEventArgs> OnCommandReceived;

        event EventHandler<NewSubscriberEventArgs> OnNewSubscriber;
        event EventHandler<UserStatusEventArgs> OnUserNoticed;
        event EventHandler<UserStatusEventArgs> OnUserLeft;
    }
}