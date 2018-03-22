using System;
using System.Threading.Tasks;
using DevChatter.Bot.Core.ChatSystems;
using DevChatter.Bot.Core.Events;

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

        public event EventHandler<CommandReceivedEventArgs> OnCommandReceived;
        public event EventHandler<NewSubscriberEventArgs> OnNewSubscriber;
        public event EventHandler<UserStatusEventArgs> OnUserLeft;
        public event EventHandler<UserStatusEventArgs> OnUserNoticed;
    }
}