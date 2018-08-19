using DevChatter.Bot.Core.Systems.Chat;
using System;
using System.Collections.Generic;

namespace DevChatter.Bot.Games.Mud.FSM.PlayStates
{
    class Level1 : RoomState
    {
        public Level1(string name, List<Actions> actionList, List<Moves> moveList, List<string> things,
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
                    CharacterInfo.StatesSeen.Add(this);
                    StateMachine.PlayInstance.AddState(
                        CharacterInfo.StatesSeen[CharacterInfo.StatesSeen.Count - 2]); // go to previous room again
                    break;
                case "west":
                    State state = new Level2("second room",
                        new List<Actions>() {Actions.Take, Actions.Look, Actions.Hide},
                        new List<Moves>() {Moves.North, Moves.South, Moves.East, Moves.West},
                        new List<string>() {"Fork", "Shovel", "Map"},
                        _chatClient);
                    StateMachine.PlayInstance.AddState(state);
                    //StateMachine.MenuInstance.AddState(new ControlOptionState("Controls"));
                    break;
                case "these":
                    //StateMachine.MenuInstance.RemoveState();

                    break;

                default:
                    break;
            }

            return run;
        }
    }
}
