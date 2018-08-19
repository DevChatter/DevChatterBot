using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Games.Mud.FSM
{
    public abstract class State
    {
        protected IChatClient _chatClient;
        public string Name { get; }

        protected State(string name, IChatClient chatClient)
        {
            Name = name;
            _chatClient = chatClient;
        }

        public abstract void Enter();

        public abstract void Exit();

        public abstract bool Run();
    }
}
