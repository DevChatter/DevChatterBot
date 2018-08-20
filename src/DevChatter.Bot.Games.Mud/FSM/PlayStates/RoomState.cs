using DevChatter.Bot.Core.Systems.Chat;
using System.Collections.Generic;

namespace DevChatter.Bot.Games.Mud.FSM.PlayStates
{
    internal abstract class RoomState : State, IContainer
    {
        protected IList<Moves> AvailableMoves;
        protected IList<Actions> AvailableActions;
        public IList<Item> Items { get; }

        protected RoomState(string name,
            IList<Actions> actionList, IList<Moves> moveList, IList<Item> items,
            IChatClient chatClient) :
            base(name, chatClient)
        {
            Items = items;
            AvailableActions = actionList;
            AvailableMoves = moveList;
        }

        public abstract override void Enter();

        public abstract override void Exit();

        public abstract override bool Run();
    }
}
