using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Games.Mud.Data.Model
{
    public class Item : DataEntity
    {
        public Room InRoom { get; set; }
        public Container InContainer { get; set; }
        public string ItemType { get; set; }
        public string Description { get; set; }
    }
}
