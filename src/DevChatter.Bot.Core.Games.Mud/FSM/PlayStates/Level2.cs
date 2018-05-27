using System;
using System.Collections.Generic;
using System.Text;

namespace DevChatter.Bot.Core.Games.Mud.FSM.PlayStates
{
    class Level2 : RoomState
    {
        public Level2(string name, List<CharacterInfo.Actions> actionList, List<CharacterInfo.Moves> moveList, List<string> things) :
            base(name, actionList, moveList, things)
        {
        }

        public override void Enter()
        {
            //Console.WriteLine("You came from the south direction, through the Window of all things");
            Console.Write("Available actions are");
            foreach (var act in availableActions)
            {
                Console.Write(", " + act.ToString());
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
