using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Games.Mud.Data.Model
{
    public class Container : DataEntity
    {
        public Room InRoom { get; set; }
        public Player OnPlayer { get; set; }
    }
}
