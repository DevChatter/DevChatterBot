using DevChatter.Bot.Core.Systems.Chat;
using System.Collections.Generic;
using DevChatter.Bot.Games.Mud.Things;

namespace DevChatter.Bot.Games.Mud.FSM.PlayStates
{
    internal abstract class RoomState : State, IContainer
    {
        protected IList<Moves> AvailableMoves;
        protected IList<Actions> AvailableActions;
        public IList<IThing> Things { get; }

        protected RoomState(string name,
            IList<Actions> actionList, IList<Moves> moveList, IList<IThing> things,
            IChatClient chatClient) :
            base(name, chatClient)
        {
            Things = things;
            AvailableActions = actionList;
            AvailableMoves = moveList;
        }

        public abstract override void Enter();

        public abstract override void Exit();

        public abstract override bool Run();
    }
}
