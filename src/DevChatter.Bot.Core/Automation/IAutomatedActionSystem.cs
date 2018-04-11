namespace DevChatter.Bot.Core.Automation
{
    public interface IAutomatedActionSystem
    {
        void AddAction(IIntervalAction actionToAdd);
        void RemoveAction(IIntervalAction actionToRemove);
        void RunNecessaryActions();
    }
}
