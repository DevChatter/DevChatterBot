using DevChatter.Bot.Core.Automation;

namespace UnitTests.Fakes
{
    public class FakeIntervalAction : IIntervalAction
    {
        public bool ShouldRun { get; set; }
        public bool ShouldNeverRunAgain { get; set; }
        public bool InvokeWasRun { get; set; } = false;

        public FakeIntervalAction(bool isTimeToRun, bool willNeverRunAgain)
        {
            ShouldRun = isTimeToRun;
            ShouldNeverRunAgain = willNeverRunAgain;
        }
        public string Name { get; } = "Fake";
        public bool IsTimeToRun()
        {
            return ShouldRun;
        }

        public void Invoke()
        {
            InvokeWasRun = true;
        }

        public bool IsDone => ShouldNeverRunAgain;
    }
}
