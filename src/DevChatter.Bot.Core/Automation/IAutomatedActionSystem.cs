namespace DevChatter.Bot.Core.Automation
{
    public interface IAutomatedActionSystem
    {
        void AddAction(IIntervalAction actionToAdd);
        void InvokeAction(string id);
    }
}
