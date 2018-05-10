namespace DevChatter.Bot.Core.Games
{
    public class JoinGameResult
    {
        protected bool Equals(JoinGameResult other)
        {
            return Success == other.Success && string.Equals(Message, other.Message);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((JoinGameResult) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Success.GetHashCode() * 397) ^ (Message != null ? Message.GetHashCode() : 0);
            }
        }

        public JoinGameResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
