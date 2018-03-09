using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events;

namespace DevChatter.Bot.Core.Messaging
{
    public class SimpleResponseMessage : DataItem, ICommandMessage
    {
        public SimpleResponseMessage()
        {
        }

        public SimpleResponseMessage(string commandText, string staticResponse, DataItemStatus dataItemStatus = DataItemStatus.Draft)
        {
            _staticResponse = staticResponse;
            CommandText = commandText;
            DataItemStatus = dataItemStatus;
        }

        private readonly string _staticResponse;
        public string CommandText { get; }
        public void Process(IChatClient triggeringClient, CommandReceivedEventArgs eventArgs)
        {
            triggeringClient.SendMessage(_staticResponse);
        }
    }
}