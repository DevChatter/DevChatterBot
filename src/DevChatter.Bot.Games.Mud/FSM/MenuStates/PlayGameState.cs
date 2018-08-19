using DevChatter.Bot.Core.Games.Mud.FSM.PlayStates;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Games.Mud.FSM.MenuStates
{
    public class PlayGameState : State
    {
        public PlayGameState(string name, IChatClient chatClient) : base(name, chatClient)
        {
        }

        public override bool Run()
        {
            while (StateMachine.PlayInstance.Update())
            {
                return true;
            }

            return true;
        }

        public override void Enter()
        {
            StateMachine.PlayInstance.AddState(new GameStartState("New Game", _chatClient));
        }

        public override void Exit()
        {
        }
    }
}
