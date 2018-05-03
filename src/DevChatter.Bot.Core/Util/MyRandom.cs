using System;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Core.Util
{
    public static class MyRandom
    {
        private static readonly Random Random = new Random();
        private static readonly object SyncLock = new object();

        public static int RandomNumber(int min, int max)
        {
            lock (SyncLock)
            {
                return Random.Next(min, max);
            }
        }

        public static (bool, T) ChooseRandomItem<T>(IList<T> choices)
        {
            if (choices.Any())
            {
                int randomNumber = MyRandom.RandomNumber(0, choices.Count);
                return (true, choices[randomNumber]);
            }

            return (false, default(T));
        }
    }
}
