using System;
using System.Collections.Generic;
using System.Text;

namespace DevChatter.Bot.Core.Games.Mud.FSM.MenuStates
{
    public class Menu
    {
        private string[] MenuItems;
        private bool Indented = false;

        public Menu(string[] menuItems, bool indented = false)
        {
            MenuItems = menuItems;
            Indented = indented;
        }


        private int DrawMenu(string[] inArray, bool indent = false)
        {
            bool loopComplete = false;
            int topOffset = Console.CursorTop;
            int bottomOffset = 0;
            int selectedItem = 0;
            ConsoleKeyInfo kb;

            Console.CursorVisible = false;

            if (indent)
                Indented = true;


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
                    {//This section is what highlights the selected item
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        if (Indented)
                        {
                            Console.WriteLine("   " + inArray[i]);
                        }
                        else
                        {

                            Console.WriteLine(" " + inArray[i]);
                        }
                        Console.ResetColor();
                    }
                    else
                    {//this section is what prints unselected items
                        if (Indented)
                        {
                            Console.WriteLine("   " + inArray[i]);
                        }
                        else
                        {
                            Console.WriteLine(" " + inArray[i]);

                        }
                    }
                }

                bottomOffset = Console.CursorTop;

                /*
				 * User input phase
				 * */

                kb = Console.ReadKey(true); //read the keyboard

                switch (kb.Key)
                { //react to input
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

        public static bool YesNo()
        {
            var yesNo = new Menu(new string[] { "Yes", "No" }, true);
            if (yesNo.PrintMenuInt() == 0) //yes returns 0, no returns 1
                return true;
            else
                return false;
        }

        public int PrintMenuInt()
        {
            return DrawMenu(MenuItems);
        }



    }
}
