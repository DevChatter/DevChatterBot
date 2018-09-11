namespace DevChatter.Bot.Core.Events.Args
{
    public class WhisperReceivedEventArgs
    {
        public string FromDisplayName { get; set; }
        public string Message { get; set; }
    }
}
