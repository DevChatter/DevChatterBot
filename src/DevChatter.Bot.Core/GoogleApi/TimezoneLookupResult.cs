namespace DevChatter.Bot.Core.GoogleApi
{
    public class TimezoneLookupResult
    {
        public string Message { get; set; }
        public string TimezoneName { get; set; }
        public int Offset { get; set; }
        public bool Success { get; set; }
    }
}
