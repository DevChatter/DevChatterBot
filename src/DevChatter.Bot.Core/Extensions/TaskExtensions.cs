using DevChatter.Bot.Core.Exceptions;
using System;
using System.Threading.Tasks;

namespace DevChatter.Bot.Core.Extensions
{
    public static class TaskExtensions
    {
        public static async Task<T> TryGetResult<T>(this Task<T> task, int retryCount = 5, Exception exception = null)
        {
            try
            {
                if (retryCount <= 0)
                {
                    throw new RetryException(exception);
                }

                return await task;
            }
            catch (Exception ex) when (!(ex is RetryException)) // allow RetryExceptions to go unhandled
            {
                await Task.Delay(500);
                return await TryGetResult(task, retryCount - 1, ex);
            }
        }
    }
}
