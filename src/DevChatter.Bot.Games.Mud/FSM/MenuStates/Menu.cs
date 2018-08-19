using System;

namespace DevChatter.Bot.Games.Mud.FSM.MenuStates
{
    public class Menu
    {
        private readonly string[] _menuItems;
        private bool _indented = false;

        public Menu(string[] menuItems, bool indented = false)
        {
            _menuItems = menuItems;
            _indented = indented;
        }


        private int DrawMenu(string[] inArray, bool indent = false)
        {
            bool loopComplete = false;
            int topOffset = Console.CursorTop;
            int bottomOffset = 0;
            int selectedItem = 0;

            Console.CursorVisible = false;

            if (indent)
            {
                _indented = true;
            }


            //this will complain if the window is not big enough
            if ((inArray.Length) > Console.WindowHeight)
            {
                throw new Exception("Too many items in the array to display");
            }

            /**
			 * Drawing phase
			 * */
            while (!loopComplete)
            {
                for (int i = 0; i < inArray.Length; i++)
                {
                    if (i == selectedItem)
                    {
                        //This section is what highlights the selected item
                        PrintSelectedItems(inArray, i);
                    }
                    else
                    {
                        //this section is what prints unselected items
                        PrintItems(inArray, i);
                    }
                }

                bottomOffset = Console.CursorTop;

                /*
				 * User input phase
				 * */

                var kb = Console.ReadKey(true);

                switch (kb.Key)
                {
                    //react to input
                    case ConsoleKey.UpArrow:
                        if (selectedItem > 0)
                        {
                            selectedItem--;
                        }
                        else
                        {
                            selectedItem = (inArray.Length - 1);
                        }

                        break;

                    case ConsoleKey.DownArrow:
                        if (selectedItem < (inArray.Length - 1))
                        {
                            selectedItem++;
                        }
                        else
                        {
                            selectedItem = 0;
                        }

                        break;

                    case ConsoleKey.Enter:
                        loopComplete = true;
                        break;
                }

                //Reset the cursor to the top of the screen
                Console.SetCursorPosition(0, topOffset);
            }

            //set the cursor just after the menu so that the program can continue after the menu
            Console.SetCursorPosition(0, bottomOffset);

            Console.CursorVisible = true;
            return selectedItem;
        }

        private void PrintItems(string[] inArray, int i)
        {
            if (_indented)
            {
                Console.WriteLine("   " + inArray[i]);
            }
            else
            {
                Console.WriteLine(" " + inArray[i]);
            }
        }

        private void PrintSelectedItems(string[] inArray, int i)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Cyan;

            PrintItems(inArray, i);

            Console.ResetColor();
        }

        public static bool YesNo()
        {
            var yesNo = new Menu(new[] {"Yes", "No"}, true);
            return yesNo.PrintMenuInt() == 0;
        }

        public int PrintMenuInt()
        {
            return DrawMenu(_menuItems);
        }
    }
}
