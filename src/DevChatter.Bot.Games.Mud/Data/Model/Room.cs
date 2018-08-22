using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Games.Mud.Data.Model
{
    public class Room : DataEntity
    {
        public Room NorthRoom { get; set; }
        public Room SouthRoom { get; set; }
        public Room EastRoom { get; set; }
        public Room WestRoom { get; set; }
    }
}
