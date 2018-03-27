using System;

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
    }
}