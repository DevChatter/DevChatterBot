using System;
using System.Collections.Generic;
using System.Text;

namespace DevChatter.Bot.Core.Games
{
    public interface IGame
    {
        bool IsRunning { get; }
    }
}
