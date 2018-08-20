using System.Collections.Generic;
using DevChatter.Bot.Games.Mud.Things;

namespace DevChatter.Bot.Games.Mud
{
    public interface IContainer
    {
        IList<IThing> Things { get; }
    }
}
