using System;
using System.Linq;

namespace DevChatter.Bot.Core.Extensions
{
    public static class IComparableExtensions
    {
        public static bool EqualsAny(this IComparable comparable, params IComparable[] others)
        {
            return others.Contains(comparable);
        }
    }
}
