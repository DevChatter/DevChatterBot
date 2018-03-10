using System;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events;

namespace DevChatter.Bot.Core.Messaging
{
    public class SimpleResponseMessage : DataItem, ICommandMessage
    {
        public SimpleResponseMessage()
        {
        }

        public SimpleResponseMessage(string commandText, string staticResponse,
            DataItemStatus dataItemStatus = DataItemStatus.Draft, 
            Func<CommandReceivedEventArgs, string> selector = null)
        {
            _staticResponse = staticResponse;
            _selector = selector;
            CommandText = commandText;
            DataItemStatus = dataItemStatus;
        }

        private readonly string _staticResponse;
        private readonly Func<CommandReceivedEventArgs, string> _selector;
        public string CommandText { get; }
        public void Process(IChatClient triggeringClient, CommandReceivedEventArgs eventArgs)
        {
            string textToSend = _staticResponse;
            if (_selector != null)
            {
                string selectedValue = _selector(eventArgs);
                textToSend = string.Format(textToSend, selectedValue);
            }
            triggeringClient.SendMessage(textToSend);
        }
    }
}