using DevChatter.Bot.Core.Systems.Chat;
using System;
using System.Collections.Generic;

namespace DevChatter.Bot.Games.Mud.FSM.PlayStates
{
    class Level2 : RoomState
    {
        public Level2(string name, List<ActionsEnum> actionList, List<Moves> moveList, List<string> things,
            IChatClient chatClient) :
            base(name, actionList, moveList, things, chatClient)
        {
        }

        public override void Enter()
        {
            //Console.WriteLine("You came from the south direction, through the Window of all things");
            Console.Write("Available actions are");
            foreach (var act in AvailableActions)
            {
                Console.Write(", " + act);
            }

            Console.WriteLine();
        }

        public override void Exit()
        {
            Console.Clear();
            Console.WriteLine("You go up to the window and climb out");
        }

        public override bool Run()
        {
            bool run = true;
            string read = Console.ReadLine().ToLower();


            switch (read)
            {
                case "south":
                    State state = CharacterInfo.StatesSeen[CharacterInfo.StatesSeen.Count - 1];
                    StateMachine.PlayInstance.RemoveState();
                    StateMachine.PlayInstance.AddState(state);
                    break;
                case "look":

                    break;
                default:
                    break;
            }

            return run;
        }
    }
}
