using System;
using System.Collections.Generic;
using System.Linq;

namespace _07312020_Task_List
{
    class Program
    {
        static void Main(string[] args)
        {
            #region startingLists
            List<string> mainMenu = new List<string>
            {
                "List Task",
                "Add Task",
                "Delete Task",
                "Mark Task Complete",
                "Quit"
            };
            List<Task> taskList = new List<Task>();

            #endregion

            bool end = false;
            int index = 0;
            while (!end)
            {
                #region MainMenu
                PrintGreen("Welcome to The J.U.N.G.L.E (Just Ugly, Not Gonna Lie, but Efficient) Task Manager.");
                index = DisplayMenu(mainMenu, "We've got tasks and things. We can do anything you want.\nEnter the number for desired option.");
                #endregion

                #region menuOptions
                //List Task
                if (index - 1 == 0)
                {
                    if (taskList.Count==0)
                    {
                        Console.Clear();
                        PrintGreen("There are currently no new tasks!\nPress any key to return to Main Menu");
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                    }
                    else
                    {
                        Console.Clear();
                        PrintGreen();
                        for (int i = 0; i < taskList.Count; i++)
                        {
                            PrintCyan($"{i+1}. ")
                        }
                    }
                }

                //Add Task
                else if (index - 1 == 1)
                {
                    taskList.Add(new Task());

                }

                //Delete Task
                else if (index - 1 == 2)
                {

                }

                //Mark Task Complete
                else if (index - 1 == 3)
                {


                }

                //Exit
                else if (index - 1 == 4)
                {

                    Console.Clear();
                    end = ContinuePlay("Are You sure you which to exit? (Y/N)");

                }
                #endregion
            }
        }
        static public int DisplayMenu(List<string> menu, string message)
        {
            int input = 0;
            for (int i = 0; i < menu.Count; i++)
            {
                PrintCyan($"{i + 1}. {menu[i]}");
            }

            input = ValidateMenuNumberInput(GetInput(message), menu);
            return input;
        }
        public static bool ContinuePlay(string message)
        {
            bool end = false;
            string cont = "";
            cont = GetInput(message).ToLower();
            while (cont.ToLower() != "n")
            {

                if (cont == "n")
                {
                    break;
                }
                else if (cont == "y")
                {
                    end = true;
                    return end;
                }
                else
                {
                   cont = GetInput("Are you sure you wish to quit? (Y to quit, N to stay)");
                }
            }
            Console.Clear();
            return end;
        }
        public static void PrintGreen(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void PrintCyan(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static string GetInput(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.White;
            string input = Console.ReadLine();
            Console.Beep();
            return input;
        }
        public static int ValidateMenuNumberInput(string input, List<string> menu)
        {
            int inputToInt = 0;

            if (!int.TryParse(input, out inputToInt))
            {
                Console.Clear();
                PrintGreen($"Not a valid input; Please enter {1}-{menu.Count} to access menu options. Press any key to return.");
                Console.ReadKey();
                Console.Clear();
            }
            else if (int.TryParse(input, out inputToInt))
            {

            }

            return inputToInt;
        }
    }

}