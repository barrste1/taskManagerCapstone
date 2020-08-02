using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

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

            #region StartingVariables
            //end will end the main loop (and thus the program) if marked true)
            bool end = false;
            
            //index is used to track menu choices
            int index = -1;

            //deleteIndex is used to store the index intended for deletion while the use confirms the deletion.
            int deleteIndex = -1;

            //Confirms deletion status of selected task.
            bool deleteConfirm = false;
            #endregion

            while (!end)
            {
                #region MainMenu

                index = DisplayMenu(mainMenu);
                #endregion

                #region menuOptions
                //List Task
                if (index - 1 == 0)
                {
                    if (taskList.Count == 0)
                    {
                        Console.Clear();
                        ReturnToMenu("There are currently no new tasks!\nPress any key to return to Main Menu");
                    }
                    else
                    {
                        Console.Clear();
                        DisplayTasks(taskList);
                        ReturnToMenu("Press any key to return to the main menu.");

                    }
                }

                //Add Task
                else if (index - 1 == 1)
                {
                    Console.Clear();
                    PrintGreen("Alright! Feel free to input the desired information!");
                    taskList.Add(new Task());


                    //Need to add validation
                    taskList[taskList.Count - 1].Name = GetInput("Person Responsible for task:");
                    taskList[taskList.Count - 1].Description = GetInput("Task to be performed: ");
                    taskList[taskList.Count - 1].DueDate = ValidateDateTime(GetInput("When is the due date: "));
                    Console.Beep(400, 200); Console.Beep(600, 400);
                    ReturnToMenu("New task successfully added! Press any key to return to the main menu!");
                }

                //Delete Task
                else if (index - 1 == 2)
                {
                    Console.Clear();
                    DisplayTasks(taskList);
                    Console.WriteLine("");

                    deleteIndex = ValidateIndex(GetInput("Which task would you like to delete?"),taskList);
                    deleteConfirm = ValidateYesNo($"Are you sure you wish to delete Task {deleteIndex}: \"{taskList[deleteIndex-1].Description}\"?");
                    
                    if (deleteConfirm)
                    {
                        taskList.RemoveAt(deleteIndex-1);
                        Console.Beep(300,200);Console.Beep(100, 600);
                        deleteConfirm = false;
                        ReturnToMenu("Task successfully deleted! Press any key to return to main menu!");
                    }
                    



                }

                //Change task completion status (from true to false or vice versa)
                else if (index - 1 == 3)
                {
                    Console.Clear();
                    DisplayTasks(taskList);
                    Console.WriteLine("");
                    index = ValidateIndex(GetInput("What task would you like to change the completion status of??"),taskList);
                    taskList[index - 1].Completed();
                    ReturnToMenu("Task marked completed! Press anykey to return to the main menu!");
                    index = -1;

                }

                //Exit
                else if (index - 1 == 4)
                {

                    Console.Clear();
                    end = ValidateYesNo("Are You sure you which to exit? (Y/N)");

                }
                #endregion
            }
        }
        static public int DisplayMenu(List<string> menu)
        {

            int input = 0;
            while (true)
            {
                PrintGreen("Welcome to The J.U.N.G.L.E (Just Ugly, Not Gonna Lie, but Efficient) Task Manager.");
                for (int i = 0; i < menu.Count; i++)
                {
                    PrintCyan($"{i + 1}. {menu[i]}");
                }
                input = ValidateNumberInput(GetInput("We've got tasks and things. We can do anything you want.\nEnter the number for desired option."));
                if (input < 1 || input > menu.Count)
                {
                    ReturnToMenu($"Please select a menu option by pressing the appropriate number (1-{menu.Count}).");
                }
                else
                {
                    break;
                }
            }

            return input;
        }
        public static bool ValidateYesNo(string message)
        {
            bool YesNo = false;
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
                    YesNo = true;
                    return YesNo;
                }
                else
                {
                    cont = GetInput("Invalid input; please enter Y or N.");
                }
            }
            Console.Clear();
            return YesNo;
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
        public static void ReturnToMenu(string message)
        {
            PrintGreen(message);
            Console.ReadKey();
            Console.Clear();
        }
        public static void DisplayTasks(List<Task> taskList)
        {
            Console.Clear();
           
            PrintGreen("Index\tName\t\tTask\t\t\t\tDue Date\tCompleted");
            PrintGreen("-----\t------\t\t-----------\t\t\t---------\t---------");
            for (int i = 0; i < taskList.Count; i++)
            {
                PrintCyan($"{i + 1}.\t{taskList[i].Name}\t\t{taskList[i].Description}\t\t{taskList[i].DueDate.ToShortDateString()}\t{taskList[i].Complete}");
            }
        }
        public static int ValidateNumberInput(string userInput)
        {
            int validNumber = -1;
            while (!int.TryParse(userInput, out validNumber))
            {
                userInput = GetInput("Invalid input; Please enter a valid number for the desired option.");

            }

            return validNumber;
        }
        public static DateTime ValidateDateTime(string userInput)
        {
            DateTime taskDate;
            
            while (!DateTime.TryParse(userInput,out taskDate))
            {
                Console.Clear();
               userInput =  GetInput("Please enter the date that the task is due.");

            }
            return taskDate;
        }
        public static int ValidateIndex(string userInput,List<Task> taskList)
        {
            int inputInt;
            while (true)
            {
                inputInt = ValidateNumberInput(userInput);
                if (inputInt>taskList.Count)
                {
                    userInput = GetInput("Please enter a number that corresponds to a task. There are not that many tasks (yet)!");
                    continue;
                }
                else if (inputInt<1)
                {
                    userInput = GetInput("Please enter a number that corresponds to a task. People are working hard! There are more tasks than you think!!");
                    continue;
                }
                else
                {
                    break;
                }
            }
            return inputInt;
        }
    }

}