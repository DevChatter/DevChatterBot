using System.Collections.Generic;
using System.Text;
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
        public List<Item> Items { get; set; } = new List<Item>();

        public string Look()
        {
            return $"You find yourself in {BasicText}. {CreateExitDescriptions()} {CreateItemsDescriptions()}";
        }

        private string CreateItemsDescriptions()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Item item in Items)
            {
                sb.Append($"You see a {item.Description}.");
            }
            return sb.ToString();
        }

        private string CreateExitDescriptions()
        {
            string northText = NorthRoom == null ? "" : "A door leading North is open.";
            string eastText = EastRoom == null ? "" : "A door leading East is open.";
            string southText = SouthRoom == null ? "" : "A door leading South is open.";
            string westText = WestRoom == null ? "" : "A door leading West is open.";

            return $"{northText} {eastText} {southText} {westText}";
        }
    }
}
