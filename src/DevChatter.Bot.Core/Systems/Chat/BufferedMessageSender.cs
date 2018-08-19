using System;
using System.Threading.Tasks;

namespace DevChatter.Bot.Core.Systems.Chat
{
    public class BufferedMessageSender : IMessageSender
    {
        private DateTime _timeLastSent = DateTime.UtcNow;
        private readonly IChatClient _internalChatClient;

        public BufferedMessageSender(IChatClient internalChatClient)
        {
            _internalChatClient = internalChatClient;
        }

        public void SendMessage(string message)
        {
            if (_timeLastSent.AddMinutes(1) > DateTime.UtcNow)
            {
                QueueTheMessage(message);
            }
            else
            {
                _internalChatClient.SendMessage(message);
                _timeLastSent = DateTime.UtcNow;
            }
        }

        private void QueueTheMessage(string message)
        {
            Task.Run(() =>
            {
                Task.Delay(TimeSpan.FromMinutes(1));
                SendMessage(message);
            });
        }

        public void SendDirectMessage(string username, string message)
        {
            _internalChatClient.SendDirectMessage(username, message);
        }
    }
}
