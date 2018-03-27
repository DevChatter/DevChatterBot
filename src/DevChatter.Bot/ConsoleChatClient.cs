using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Model;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot
{
    public class ConsoleChatClient : IChatClient
    {
        public Task Connect()
        {
            return Task.CompletedTask;
        }

        public Task Disconnect()
        {
            return Task.CompletedTask;
        }

        public void SendMessage(string message)
        {
            Console.WriteLine(message);
        }

        public List<ChatUser> GetAllChatters()
        {
            return new List<ChatUser>();
        }

        public event EventHandler<CommandReceivedEventArgs> OnCommandReceived;
        public event EventHandler<NewSubscriberEventArgs> OnNewSubscriber;
        public event EventHandler<UserStatusEventArgs> OnUserLeft;
        public event EventHandler<UserStatusEventArgs> OnUserNoticed;
    }
}