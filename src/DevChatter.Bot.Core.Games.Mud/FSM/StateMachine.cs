using System;
using System.Collections.Generic;
using System.Text;

namespace DevChatter.Bot.Core.Games.Mud.FSM
{
        public class StateMachine
        {
            Stack<State> states = new Stack<State>();


            /// <summary>
            /// DIS IS SINGLETON
            /// </summary>
            static StateMachine menuInstance;
            static StateMachine playInstance;
            static StateMachine actionInstance;

            public static StateMachine MenuInstance
            {
                get
                {
                    if (menuInstance == null)
                    {
                        menuInstance = new StateMachine();
                    }
                    return menuInstance;
                }
            }

            public static StateMachine PlayInstance
            {
                get
                {
                    if (playInstance == null)
                    {
                        playInstance = new StateMachine();
                    }
                    return playInstance;
                }
            }

            public static StateMachine ActionInstance
            {
                get
                {
                    if (actionInstance == null)
                    {
                        actionInstance = new StateMachine();
                    }
                    return actionInstance;
                }
            }

            public void AddState(State state)
            {
                state.Exit();
                states.Push(state);
                state.Enter();
            }

            public void RemoveState()
            {
                State state = states.Peek();
                state.Exit();


                states.Pop();
                State nState = states.Peek();
                nState.Enter();

            }

            public bool Update()
            {

                State state = states.Peek();
                return state.Run();
            }

        }
    }
