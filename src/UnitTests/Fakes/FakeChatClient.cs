using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevChatter.Bot.Core.ChatSystems;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Model;

namespace UnitTests.Fakes
{
    public class FakeChatClient : IChatClient
    {
        public Task Connect()
        {
            throw new NotImplementedException();
        }

        public Task Disconnect()
        {
            throw new NotImplementedException();
        }

        public void SendMessage(string message)
        {
            SentMessage = message;
        }

        public List<ChatUser> GetAllChatters()
        {
            throw new NotImplementedException();
        }

        public string SentMessage { get; set; }

        public event EventHandler<CommandReceivedEventArgs> OnCommandReceived;
        public event EventHandler<NewSubscriberEventArgs> OnNewSubscriber;
        public event EventHandler<UserStatusEventArgs> OnUserNoticed;
        public event EventHandler<UserStatusEventArgs> OnUserLeft;
    }
}