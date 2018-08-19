using System;
using System.Collections.Generic;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Games.Mud.FSM.PlayStates
{
    class GameStartState : State
    {
        private readonly Dictionary<string, string> _itemsHere;

        public GameStartState(string name, IChatClient chatClient) : base(name, chatClient)
        {
            _itemsHere = new Dictionary<string, string>
            {
                ["lamp"] = "a Lamp sits on the table, ",
            };
        }

        public override void Enter()
        {
            Console.Clear();
            Console.WriteLine("You see an empty room");
            Console.WriteLine(
                "Available actions are\n\"Look\", \"Take\", \"Inventory\", \"Use\", \"Equipped\", \"Put\" \nand all movement, north, south, east & west");
        }

        public override void Exit()
        {
        }

        public override bool Run()
        {
            string read = Console.ReadLine()?.ToLower();

            CharacterInfo action = new CharacterInfo();
            if (read.Contains("take"))
            {
                int index = read.IndexOf("lamp");
                int i = (int) index;
                string thing = read.Substring(i, 4);
                Console.WriteLine($"You reach out and try to take the {thing}.");
                Console.WriteLine(
                    $"Your fingers gently lift the {thing} and you are now the proud owner of it.\nGood job you!");
                CharacterInfo.Inventory.Add(thing);
                _itemsHere.Remove(thing);
            }
            else if (read.Contains("use") && CharacterInfo.Inventory.Count >= 1)
            {
                if (read.Contains("lamp"))
                {
                    int index = read.IndexOf("lamp");
                    string thing = read.Substring(index, 4);
                    Console.WriteLine($"You take the {thing} out of your bag");
                    Console.WriteLine($"The room is now a no longer as dark as it was a moment ago.\nYou feel safer.");
                    CharacterInfo.Inventory.Remove(thing);
                    CharacterInfo.Equipped.Add(thing);
                }
                else
                {
                    Console.WriteLine("Terribly sorry sir, You do not have that item.");
                }
            }
            else if (read.Contains("use"))
            {
                Console.WriteLine($"Terribly sorry sir, but I am afraid you do not have any items yet...");
            }

            else
            {
                switch (read)
                {
                    case "look":
                        if (_itemsHere.Count >= 1)
                        {
                            Console.WriteLine(
                                $"You see {_itemsHere["lamp"]} an open window to the north, a closed door to the west.");
                            break;
                        }

                        Console.WriteLine($"You see an open window to the north, a closed door to the west.");
                        break;

                    case "north":
                        CharacterInfo.StatesSeen.Add(this);
                        StateMachine.PlayInstance.AddState(new Level1("First room",
                            new List<Actions>() {Actions.Look, Actions.Take},
                            new List<Moves>() {Moves.North, Moves.South, Moves.West},
                            new List<string>() {"a Window", "Some pebbles", "a sword"},
                            _chatClient));
                        break;
                    case "west":
                        Console.WriteLine("You approach the door, it seems blocked, you are unable to move it.");
                        break;
                    case "south":
                        Console.WriteLine(
                            "You turn around, and try to walk southwards,\nhow you know what is south is mindboggling, but that is not important right now,\nYou stare at a wall, how did you get here?");
                        break;
                    case "inventory":
                        if (CharacterInfo.Inventory.Count >= 1)
                        {
                            Console.WriteLine($"You carry around these things");
                            foreach (var item in CharacterInfo.Inventory)
                            {
                                Console.WriteLine($"-> {item}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Terribly sorry sir, you do not have any items.");
                        }

                        break;
                    case "equipped":
                        if (CharacterInfo.Equipped.Count >= 1)
                        {
                            Console.WriteLine($"Currently on your person, hands or otherwise:");
                            foreach (var item in CharacterInfo.Equipped)
                            {
                                Console.WriteLine($"-> {item}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("You do not have anything equipped yet.");
                        }

                        break;
                    case "east":
                        Console.WriteLine("It is very dark to the east, you should not go there");
                        break;
                    case "":
                        Console.WriteLine($"Terribly sorry sir, I did not quite hear that.");
                        break;
                    default:
                        Console.WriteLine($"Terribly sorry sir, I do not understand what you mean by \"{read}\".");
                        break;
                }
            }

            //action.actionDict.Add(Action.Actions.)
            return true;
        }
    }
}
