namespace DevChatter.Bot.Core.Games.Mud.FSM
{
    public abstract class State
    {
        public string Name { get; }

        protected State(string name)
        {
            Name = name;
        }

        public abstract void Enter();

        public abstract void Exit();

        public abstract bool Run();
    }
}
