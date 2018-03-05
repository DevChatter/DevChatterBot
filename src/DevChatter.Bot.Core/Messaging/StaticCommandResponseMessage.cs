using System;

namespace DevChatter.Bot.Core.Messaging
{
    public class StaticCommandResponseMessage : ICommandMessage
    {
        private readonly string _staticResponse;
        public string CommandText { get; }
        public void Process(IChatClient triggeringClient, CommandReceivedEventArgs eventArgs)
        {
            triggeringClient.SendMessage(_staticResponse);
        }

        public StaticCommandResponseMessage(string commandText, string staticResponse, DataItemStatus dataItemStatus = DataItemStatus.Draft)
        {
            _staticResponse = staticResponse;
            CommandText = commandText;
            DataItemStatus = dataItemStatus;
        }

        public DataItemStatus DataItemStatus { get; }
    }
}