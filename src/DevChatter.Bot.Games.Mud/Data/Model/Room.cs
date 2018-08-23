using System.Collections.Generic;
using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Games.Mud.Data.Model
{
    public class Room : DataEntity
    {
        public Room NorthRoom { get; set; }
        public Room SouthRoom { get; set; }
        public Room EastRoom { get; set; }
        public Room WestRoom { get; set; }
        public List<Item> Items { get; set; }

        public string Look()
        {
            return $"You find yourself in a small room. A door leading North is open. You see a stack of torches on the ground.";
        }
    }
}
