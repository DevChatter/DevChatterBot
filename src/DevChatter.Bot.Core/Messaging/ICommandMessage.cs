namespace DevChatter.Bot.Core.Messaging
{
    public interface ICommandMessage : IDataItem
    {
        string CommandText { get; }
        void Process(IChatClient triggeringClient, CommandReceivedEventArgs eventArgs);

    }
}