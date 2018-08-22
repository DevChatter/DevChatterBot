using System.Collections.Generic;
using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Games.Mud.Data.Model
{
    public class Player : DataEntity
    {
        public string Name { get; set; }
        public Room InRoom { get; set; }
        public List<Container> Containers { get; set; }
    }
}
