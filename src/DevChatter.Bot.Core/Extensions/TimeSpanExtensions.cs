using System;

namespace DevChatter.Bot.Core.Extensions
{
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Returns a formatted TimeSpan
        /// If there are more than 0 days it formats like: 1.00:00:00
        /// If there are more than 0 hours it formats like: 1:00:00
        /// Otherwise formatted like: 00:00
        /// </summary>
        /// <param name="timeSpan"></param>
        public static string ToExpandingString(this TimeSpan timeSpan)
        {
            if(timeSpan.Days > 0)
            {
                return timeSpan.ToString("d\\.hh\\:mm\\:ss");
            }
            else if (timeSpan.Hours > 0)
            {
                return timeSpan.ToString("hh\\:mm\\:ss");
            }
            else
            {
                return timeSpan.ToString("mm\\:ss");
            }
        }
    }
}
