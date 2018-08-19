using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevChatter.Bot.Core.Systems.Chat
{
    public interface IChatClient : IMessageSender
    {
        Task Connect();

        Task Disconnect();

        void Timeout(string username, TimeSpan duration, string reason);
        void Ban(string username, string reason);

        IList<ChatUser> GetAllChatters();

        event EventHandler<CommandReceivedEventArgs> OnCommandReceived;
        event EventHandler<NewSubscriberEventArgs> OnNewSubscriber;
        event EventHandler<UserStatusEventArgs> OnUserNoticed;
        event EventHandler<UserStatusEventArgs> OnUserLeft;
    }
}
