using System;

namespace DevChatter.Bot.Core.Util
{
    public interface IClock
    {
        DateTime UtcNow { get; }
        DateTime Now { get; }
    }

    public class SystemClock : IClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
        public DateTime Now => DateTime.Now;
    }
}
