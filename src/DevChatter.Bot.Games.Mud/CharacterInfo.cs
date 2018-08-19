using DevChatter.Bot.Games.Mud.FSM;
using System.Collections.Generic;

namespace DevChatter.Bot.Games.Mud
{
    public class CharacterInfo
    {
        public Dictionary<Moves, string> MoveDict = new Dictionary<Moves, string>();
        public Dictionary<Actions, string> ActionDict = new Dictionary<Actions, string>();

        public List<string> RoomsVisited = new List<string>();
        public static List<string> Inventory = new List<string>();
        public static List<string> Equipped = new List<string>();

        public static bool SoundEnabled = true;

        public static List<State> StatesSeen = new List<State>();
    }
}
