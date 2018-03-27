using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Games.RockPaperScissors
{
    public class RockPaperScissors
    {
        public static RockPaperScissors Rock = new RockPaperScissors(0, nameof(Rock));
        public static RockPaperScissors Paper = new RockPaperScissors(1, nameof(Paper));
        public static RockPaperScissors Scissors = new RockPaperScissors(2, nameof(Scissors));

        private RockPaperScissors(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public int Id { get; }
        public string Name { get; }

        public RockPaperScissors LosesTo()
        {
            return GetById((Id + 1) % 3);
        }

        public static List<RockPaperScissors> All { get; } = new List<RockPaperScissors> {Rock, Paper, Scissors};

        public static RockPaperScissors GetById(int id)
        {
            return All.SingleOrDefault(x => x.Id == id);
        }

        public static bool GetByName(string name, out RockPaperScissors value)
        {
            value = All.SingleOrDefault(x => string.Compare(x.Name, name, StringComparison.OrdinalIgnoreCase) == 0);
            return value != null;
        }

        public static RockPaperScissors GetRandomChoice()
        {
            return GetById(MyRandom.RandomNumber(0, 3));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}