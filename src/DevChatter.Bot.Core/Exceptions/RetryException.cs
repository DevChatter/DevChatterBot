using System;

namespace DevChatter.Bot.Core.Exceptions
{
    public class RetryException : Exception
    {
        public RetryException(Exception innerException)
            : base(innerException?.Message, innerException)
        {
        }
    }
}
