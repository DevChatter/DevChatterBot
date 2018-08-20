using System.Collections.Generic;

namespace DevChatter.Bot.Games.Mud
{
    public interface IContainer
    {
        IList<Item> Items { get; }
    }
}
