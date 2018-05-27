using System.Collections.Generic;

namespace DevChatter.Bot.Core.Games.Mud.FSM.PlayStates
{
    abstract class RoomState : State
    {
        protected List<string> ThingsInRoom;
        protected List<CharacterInfo.Moves> AvailableMoves;
        protected List<CharacterInfo.Actions> AvailableActions;

        protected RoomState(string name, List<CharacterInfo.Actions> actionList, List<CharacterInfo.Moves> moveList, List<string> things) : base(name)
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
