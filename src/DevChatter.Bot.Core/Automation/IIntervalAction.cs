namespace DevChatter.Bot.Core.Automation
{
    public interface IIntervalAction
    {
        bool IsTimeToRun();
        void Invoke();
    }
}
