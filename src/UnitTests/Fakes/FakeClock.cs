using System;
using DevChatter.Bot.Core.Util;

namespace UnitTests.Fakes
{
    public class FakeClock : IClock
    {
        public DateTime UtcNow { get; set; } = DateTime.UtcNow;
        public DateTime Now { get; set; } = DateTime.Now;
    }
}