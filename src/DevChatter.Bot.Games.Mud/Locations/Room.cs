using System.Collections.Generic;
using DevChatter.Bot.Games.Mud.Things;

namespace DevChatter.Bot.Games.Mud.Locations
{
    public class Room : IContainer
    {
        public IList<IThing> Things { get; set; }
        public IList<Moves> AvailableMoves { get; set; }
    }
}
