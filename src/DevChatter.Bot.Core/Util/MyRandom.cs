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

        public static (bool Success, T ChosenItem) ChooseRandomItem<T>(IList<T> choices)
        {
            if (choices.Any())
            {
                int randomNumber = RandomNumber(0, choices.Count);
                return (true, choices[randomNumber]);
            }

            return (false, default(T));
        }

        public static T ChooseRandomWeightedItem<T>(IList<T> weightedItems)
            where T : IWeightedItem
        {
            List<T> fullSet = new List<T>();
            foreach (T weightedItem in weightedItems)
            {
                for (int i = 0; i < weightedItem.Weight; i++)
                {
                    fullSet.Add(weightedItem);
                }
            }

            (bool success, T chosenItem) = ChooseRandomItem(fullSet);
            return success ? chosenItem : default(T);
        }
    }
}
