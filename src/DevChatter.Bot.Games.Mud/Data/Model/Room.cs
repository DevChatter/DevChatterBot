using System.Collections.Generic;
using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Games.Mud.Data.Model
{
    public class Room : DataEntity
    {
        public string BasicText { get; set; }
        public Room NorthRoom { get; set; } // TODO: Make these `Exit` objects
        public Room SouthRoom { get; set; }
        public Room EastRoom { get; set; }
        public Room WestRoom { get; set; }
        public List<Item> Items { get; set; }

        public string Look()
        {
            return $"You find yourself in {BasicText}. A door leading North is open. You see a stack of torches on the ground.";
        }

    }
}
