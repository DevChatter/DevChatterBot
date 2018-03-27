using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Model;

namespace DevChatter.Bot.Core.Systems.Chat
{
    public interface IChatClient
    {
        Task Connect();

        Task Disconnect();

        void SendMessage(string message);

        List<ChatUser> GetAllChatters();

        event EventHandler<CommandReceivedEventArgs> OnCommandReceived;

        event EventHandler<NewSubscriberEventArgs> OnNewSubscriber;
        event EventHandler<UserStatusEventArgs> OnUserNoticed;
        event EventHandler<UserStatusEventArgs> OnUserLeft;
    }
}