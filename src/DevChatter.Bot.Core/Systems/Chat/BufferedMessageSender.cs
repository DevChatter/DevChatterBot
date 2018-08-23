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
            if (DateTime.UtcNow > _timeLastSent.AddMinutes(1))
            {
                _internalChatClient.SendMessage(message);
                _timeLastSent = DateTime.UtcNow;
            }
            else
            {
                QueueTheMessage(message);
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
