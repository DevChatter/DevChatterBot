using DevChatter.Bot.Games.Mud.FSM;
using System.Collections.Generic;
using DevChatter.Bot.Games.Mud.Things;

namespace DevChatter.Bot.Games.Mud
{
    public class CharacterInfo
    {
        public Dictionary<Moves, string> MoveDict = new Dictionary<Moves, string>();
        public Dictionary<Actions, string> ActionDict = new Dictionary<Actions, string>();

        public List<string> RoomsVisited = new List<string>();
        public static List<IHoldable> Inventory = new List<IHoldable>();
        public static List<IEquippable> Equipped = new List<IEquippable>();

        public IContainer InContainer { get; set; }

        public static List<State> StatesSeen = new List<State>();
    }
}
