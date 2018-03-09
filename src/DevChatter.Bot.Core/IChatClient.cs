using System;
using System.Threading.Tasks;

namespace DevChatter.Bot.Core
{
    public interface IChatClient
    {
        Task Connect();
        void SendMessage(string message);
        event EventHandler<CommandReceivedEventArgs> OnCommandReceived;
        event EventHandler<NewSubscriberEventArgs> OnNewSubscriber;
    }
}