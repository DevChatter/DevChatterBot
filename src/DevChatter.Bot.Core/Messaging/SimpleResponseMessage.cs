using System;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Model;

namespace DevChatter.Bot.Core.Messaging
{
    public class SimpleResponseMessage : DataItem, ICommandMessage
    {
        public SimpleResponseMessage()
        {
        }

        public SimpleResponseMessage(string commandText, string staticResponse, UserRole roleRequired = UserRole.Everyone, DataItemStatus dataItemStatus = DataItemStatus.Active,
            Func<CommandReceivedEventArgs, string> selector = null)
        {
            _staticResponse = staticResponse;
            RoleRequired = roleRequired;
            _selector = selector;
            CommandText = commandText;
            DataItemStatus = dataItemStatus;
        }

        protected readonly string _staticResponse;
        protected readonly Func<CommandReceivedEventArgs, string> _selector;
        public UserRole RoleRequired { get; }
        public string CommandText { get; }
        public virtual void Process(IChatClient triggeringClient, CommandReceivedEventArgs eventArgs)
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