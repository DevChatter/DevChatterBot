using System;

namespace DevChatter.Bot.Core.Automation
{
    public class RepeatingCallbackAction : IIntervalAction
    {
        private readonly Action _callback;
        private readonly int _intervalInSeconds;

        public RepeatingCallbackAction(Action callback, int intervalInSeconds = 5)
        {
            _callback = callback;
            _intervalInSeconds = intervalInSeconds;
            NextRunTime = DateTime.UtcNow.AddSeconds(_intervalInSeconds);
        }
        public DateTime NextRunTime { get; set; }
        public bool IsTimeToRun() => DateTime.UtcNow > NextRunTime;

        public void Invoke()
        {
            try
            {
                _callback.Invoke();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            NextRunTime = DateTime.UtcNow.AddSeconds(_intervalInSeconds);
        }

        public bool IsDone { get; } = false;
    }
}
