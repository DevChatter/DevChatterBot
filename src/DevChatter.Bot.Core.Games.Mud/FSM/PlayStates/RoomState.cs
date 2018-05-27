using System.Collections.Generic;

namespace DevChatter.Bot.Core.Games.Mud.FSM.PlayStates
{
    internal abstract class RoomState : State
    {
        protected List<string> ThingsInRoom;
        protected List<Moves> AvailableMoves;
        protected List<Actions> AvailableActions;

        protected RoomState(string name, List<Actions> actionList, List<Moves> moveList, List<string> things) : base(name)
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
