using System.Collections.Generic;

namespace DevChatter.Bot.Games.Mud.FSM
{
    public class StateMachine
    {
        private readonly Stack<State> _states = new Stack<State>();

        private static StateMachine _menuInstance;
        public static StateMachine MenuInstance => _menuInstance ?? (_menuInstance = new StateMachine());

        private static StateMachine _playInstance;
        public static StateMachine PlayInstance => _playInstance ?? (_playInstance = new StateMachine());

        private static StateMachine _actionInstance;
        public static StateMachine ActionInstance => _actionInstance ?? (_actionInstance = new StateMachine());

        public void AddState(State state)
        {
            state.Exit();
            _states.Push(state);
            state.Enter();
        }

        public void RemoveState()
        {
            State state = _states.Peek();
            state.Exit();

            _states.Pop();
            State nState = _states.Peek();
            nState.Enter();
        }

        public bool Update()
        {
            State state = _states.Peek();
            return state.Run();
        }
    }
}
