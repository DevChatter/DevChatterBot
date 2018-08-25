using DevChatter.Bot.Core.Systems.Chat;
using System.Collections.Generic;

namespace DevChatter.Bot.Games.Mud.FSM.PlayStates
{
    internal abstract class RoomState : State
    {
        protected IList<string> ThingsInRoom;
        protected IList<Moves> AvailableMoves;
        protected IList<ActionsEnum> AvailableActions;

        protected RoomState(string name,
            IList<ActionsEnum> actionList, IList<Moves> moveList, IList<string> things,
            IChatClient chatClient) :
            base(name, chatClient)
        {
            ThingsInRoom = things;
            AvailableActions = actionList;
            AvailableMoves = moveList;
        }

        public abstract override void Enter();

        public abstract override void Exit();

        public abstract override bool Run();
    }
}
