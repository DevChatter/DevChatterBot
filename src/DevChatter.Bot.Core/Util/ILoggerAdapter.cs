using System;

namespace DevChatter.Bot.Core.Util
{
    public interface ILoggable<T>
    {
        void LogInformation(string message);
        void LogError(Exception ex, string message, params object[] args);
    }
}
