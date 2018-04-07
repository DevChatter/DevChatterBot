namespace DevChatter.Bot.Core.Games.Heist
{
    public class JoinGameResult
    {
        public JoinGameResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}