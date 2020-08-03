using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace _07312020_Task_List
{
    class Program
    {
        static void Main(string[] args)
        {

            #region startingLists
            List<string> mainMenu = new List<string>
            {
                "List Tasks",
                "Search Tasks",
                "Modify Task",
                "Add Task",
                "Delete Task",
                "Change Completion Status",
                "Quit"
            };

            List<string> taskProperties = new List<string>
            {
                "Name",
                "Description",
                "Due Date",
            };

            List<Task> taskList = new List<Task>()
            {
                new Task("Martin","Feeding Barry the Goldfish",DateTime.Parse("7/7/2020"),false),
                new Task("Martin","Bury Barry",DateTime.Parse("7/10/2020"),true),
                new Task("Martin","Buy fresh fish Fred",DateTime.Parse("7/10/2020"),true),
                new Task("Martin","Assign Alice active feeding duty",DateTime.Parse("7/10/2020"),true),
                new Task("Alice","Feeding Fred the Goldfish",DateTime.Parse("7/10/2020"),true),
                new Task("Martin","Walking the ferrets",DateTime.Parse("8/7/2020"),true),
                new Task("Julia","Dance in the moonlight",DateTime.Parse("7/14/2020"),true),
                new Task("Julia","Be out of sight",DateTime.Parse("7/25/2020"),true),
                new Task("Julia","Don't bark, don't bite",DateTime.Parse("8/12/2020"),true),
                new Task("Julia","Keep things loose keep things light",DateTime.Parse("8/20/2020"),false),
                new Task("Alice","Find replacement for Martin",DateTime.Parse("8/21/2020"),false),
                new Task("Julia","Stop the heat death of the universe",DateTime.Parse("8/25/9999"),false),
            };

            List<int> searchFilteredIndexes = new List<int>();

            List<string> names = new List<string>();


            #endregion

            #region StartingVariables
            //end will end the main loop (and thus the program) if marked true)
            bool end = false;

            //index is used to track menu choices
            int index = -1;

            //deleteIndex is used to store the index intended for deletion while the use confirms the deletion.
            int deleteIndex = -1;

            //Confirms deletion status of selected task.
            bool changeConfirm = false;

            //Tracks the index of the task to be modified
            int modify = -1;

            //Used to modify aspects of a task (1=Name change, 2=Description change, 3=dueDate change)
            int taskProperty = -1;

            //search variable
            int searchIndex = -1;

            //The name that the user types in to search the task database
            string searchName = "";

            //Variable used to store userinput for date search function
            DateTime searchDate;

            #endregion

            //while loop that allows program to loop until exit is declared by user.
            while (!end)
            {
                #region MainMenu

                index = DisplayMenu(mainMenu);
                #endregion

                #region menuOptions
                //List Task
                if (index == 1)
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

                //Search Function
                else if (index == 2)
                {
                    Console.Clear();

                    if (taskList.Count == 0)
                    {
                        ReturnToMenu("No tasks to search through! Press any key to return to the main menu.");
                    }
                    else
                    {
                        while (true)
                        {
                            searchIndex = ValidateNumberInput(GetInput("Search by name (1) or tasks due before a date (2). Press (0) to return to the main menu.\n"));
                            if (searchIndex == 1 || searchIndex == 2 || searchIndex == 0)
                            {
                                break;
                            }
                            else
                            {
                                Console.Beep(10000, 400);
                                PrintGreen("Invalid number input.");
                            }
                        }

                        //return to main menu
                        if (searchIndex == 0)
                        {
                            ReturnToMenu("Returning to main menu. Press any key to continue.");
                        }

                        //search by name conditional
                        else if (searchIndex == 1)
                        {
                            Console.Clear();
                            Console.Beep(300, 100); Console.Beep(700, 100); Console.Beep(300, 100);
                            names = DisplayNames(taskList);
                            searchName = ValidateName(GetInput("Who would you like to search for? Please input a name from those listed above."), names);
                            Console.Clear();
                            for (int i = 0; i < taskList.Count; i++)
                            {
                                if (taskList[i].Name.ToLower() == searchName.ToLower())
                                {
                                    searchFilteredIndexes.Add(i);
                                }
                            }

                            Console.Clear();
                            DisplaySearchTasks(taskList, searchFilteredIndexes);

                            //displays # of search results
                            ReturnToMenu($"There are {searchFilteredIndexes.Count} tasks assigned to {searchName}. Press any key to return to the main menu.");

                            //resets lists used for search function
                            names.Clear(); searchFilteredIndexes.Clear();
                        }

                        //Search by dueDate conditional
                        else if (searchIndex == 2)
                        {
                            Console.Clear();

                            searchDate = ValidateDateTime(GetInput("Please input a date in the format mm/dd/yyyy. Only tasks due before that date will be displayed."));
                            for (int i = 0; i < taskList.Count; i++)
                            {
                                if (taskList[i].DueDate<searchDate) 
                                {
                                    searchFilteredIndexes.Add(i);
                                }
                            }

                            Console.Clear();
                            DisplaySearchTasks(taskList, searchFilteredIndexes);

                            //displays number of search results
                            ReturnToMenu($"There are {searchFilteredIndexes.Count} tasks with a due date before {searchDate}. Press any key to ");

                            //resets lists used for search function
                            names.Clear(); searchFilteredIndexes.Clear();
                        }
                    }


                }

                //Modify Task
                else if (index == 3)
                {
                    if (taskList.Count == 0)
                    {
                        ReturnToMenu("No tasks to modify! Press any key to return to the main menu.");
                    }
                    else
                    {
                        Console.Clear();
                        DisplayTasks(taskList);
                        Console.WriteLine("");


                        modify = ValidateIndex(GetInput($"Which task would you like to modify? Input the index number list above (1-{taskList.Count})."), taskList);

                        //This shows only the task to be modified, to reduce visual clutter for the user
                        Console.Clear();
                        DisplayChosenTask(taskList, modify);

                        //This is just formatting to show visually which fields the user is going to change
                        PrintYellow("----------------------------------------------------------------------------------------------------------------");
                        PrintYellow(String.Format("|    {0,-5} |  {1,-14} |  {2,-33} | {3,-12} |", "", "  \t1", "   \t\t2", "     3"));
                        PrintYellow("----------------------------------------------------------------------------------------------------------------");

                        taskProperty = ValidatePropertyIndex(GetInput("What would you like to modify?\nPerson Responsible = (1) , Task Assigned = (2) , Due Date = (3). Press (0) to return to main menu.\n"), taskProperties);


                        //Various options to change properties of the Task
                        #region modifyProperties
                        if (taskProperty == 0)
                        {
                            ReturnToMenu("Returning to main menu. Press any key to proceed.");
                        }
                        else if (taskProperty == 1)
                        {
                            Console.Clear();
                            DisplayChosenTask(taskList, modify);

                            taskList[modify - 1].Name = GetInput("Alright! Who is now responsible for this task?\n");

                            Console.Clear();
                            DisplayChosenTask(taskList, modify);
                            ReturnToMenu($"Task successfully updated! {taskList[modify - 1].Name} is now in charge of \"{taskList[modify - 1].Description}\".\nPress any key to return to the main menu.");
                        }
                        else if (taskProperty == 2)
                        {
                            Console.Clear();
                            DisplayChosenTask(taskList, modify);

                            taskList[modify - 1].Description = ValidateTaskString(GetInput($"Alright! What is the new task for {taskList[modify - 1].Name}?\n"));

                            Console.Clear();
                            DisplayChosenTask(taskList, modify);
                            ReturnToMenu($"Task successfully updated! {taskList[modify - 1].Name} is now in charge of \"{taskList[modify - 1].Description}\".\nPress any key to return to the main menu.");
                        }
                        else if (taskProperty == 3)
                        {
                            Console.Clear();
                            DisplayChosenTask(taskList, modify);

                            taskList[modify - 1].DueDate = ValidateDateTime(GetInput($"Alright! When is the new due date for \"{taskList[modify - 1].Description}\"? Please input (mm/dd/yyyy).\n"));

                            Console.Clear();
                            DisplayChosenTask(taskList, modify);
                            ReturnToMenu($"Task successfully updated! \"{taskList[modify - 1].Description}\" is now in due on \"{taskList[modify - 1].DueDate.ToShortDateString()}\".\nPress any key to return to main menu");
                        }
                        #endregion

                    }

                }

                //Add Task
                else if (index == 4)
                {
                    Console.Clear();
                    PrintGreen("Alright! Feel free to input the desired information!");
                    taskList.Add(new Task());


                    taskList[taskList.Count - 1].Name = GetInput("Person Responsible for task:");
                    taskList[taskList.Count - 1].Description = ValidateTaskString(GetInput("Task to be performed: "));
                    taskList[taskList.Count - 1].DueDate = ValidateDateTime(GetInput("When is the due date (mm/dd/yyyy): "));


                    Console.Clear();
                    Console.Beep(400, 200); Console.Beep(600, 400);
                    DisplayChosenTask(taskList, taskList.Count);
                    ReturnToMenu("New task successfully added! Press any key to return to the main menu!");
                }

                //Delete Task
                else if (index == 5)
                {
                    if (taskList.Count == 0)
                    {
                        ReturnToMenu("Currently no tasks in the manager. You did a good job deleting them all!");
                    }
                    else
                    {
                        Console.Clear();
                        DisplayTasks(taskList);
                        Console.WriteLine("");

                        deleteIndex = ValidateIndex(GetInput($"Which task would you like to delete? Please input index number listed above (1-{taskList.Count}"), taskList);
                        Console.Clear();
                        DisplayChosenTask(taskList, deleteIndex);
                        changeConfirm = ValidateYesNo($"Are you sure you wish to delete Task {deleteIndex}: \"{taskList[deleteIndex - 1].Description}\"? (Y/N)\n");

                        if (changeConfirm)
                        {
                            taskList.RemoveAt(deleteIndex - 1);
                            Console.Beep(300, 200); Console.Beep(100, 600);
                            changeConfirm = false;
                            ReturnToMenu("Task successfully deleted! Press any key to return to main menu!");
                        }
                    }

                }

                //Change task completion status (from true to false or vice versa)
                else if (index == 6)
                {
                    Console.Clear();
                    DisplayTasks(taskList);
                    Console.WriteLine("");

                    index = ValidateIndex(GetInput($"What task would you like to change the completion status of? Input the index number (1-{taskList.Count})."), taskList);
                    taskList[index - 1].Completed();
                    DisplayChosenTask(taskList, index);

                    if (ValidateYesNo("Confirm modification to this task (Y/N)?"))
                    {
                        ReturnToMenu("Task completion status changed! Press any key to return to the main menu!");
                    }

                    else
                    {
                        taskList[index - 1].Completed();
                        Console.Clear();
                    }

                    index = -1;

                }

                //Exit
                else if (index == 7)
                {

                    Console.Clear();
                    end = ValidateYesNo("Are You sure you which to exit? (Y/N)");

                }
                #endregion
            }
        }



        #region Text Color Change Methods
        public static void PrintCyan(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void PrintGreen(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void PrintYellow(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        #endregion

        #region QoL Methods 
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
        #endregion

        #region Methods involved in displaying tables
        public static void DisplayChosenTask(List<Task> taskList, int input)
        {
            //Displays only the chose task to user
            input--;
            PrintCyan(String.Format("|  {0,-7} |    {1,-12} | {2,-31} |   {3,-10} | {4,4}", "Index", "\tName", "\t\t\tTask", "Due Date", "Completed"));
            PrintCyan(String.Format("|{0,-6}|{1,-21}|{2,-46}|{3,-4}|{4,4}", "----------", "------------------------", "------------------------------------------------", "--------------", "-----------"));
            PrintCyan(String.Format("|    {0,-5} |  {1,-21} |  {2,-45} |  {3,-11} | {4,6}", (input + 1), taskList[input].Name, taskList[input].Description, taskList[input].DueDate.ToShortDateString(), taskList[input].Complete));
            PrintCyan("----------------------------------------------------------------------------------------------------------------");
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
        public static List<string> DisplayNames(List<Task> tasklist)
        {
            List<string> names = new List<string>();

            for (int i = 0; i < tasklist.Count; i++)
            {
                if (!names.Contains(tasklist[i].Name))
                {
                    names.Add(tasklist[i].Name);
                }
            }
            PrintYellow("Names found in database:");
            for (int i = 0; i < names.Count; i++)
            {
                PrintCyan($"{i + 1}. {names[i]}");
            }
            return names;
        }
        public static void DisplaySearchTasks(List<Task> taskList, List<int> indexes)
        {
            //Display tasks based on search results. Creates Table.

            Console.Clear();

            PrintCyan(String.Format("|  {0,-7} |    {1,-12} | {2,-31} |   {3,-10} | {4,4}", "Index", "\tName", "\t\t\tTask", "Due Date", "Completed"));
            PrintCyan(String.Format("|{0,-6}|{1,-21}|{2,-46}|{3,-4}|{4,4}", "----------", "------------------------", "------------------------------------------------", "--------------", "-----------"));


            for (int i = 0; i < taskList.Count; i++)
            {
                if (indexes.Contains(i))
                {
                    PrintCyan(String.Format("|    {0,-5} |  {1,-21} |  {2,-45} |  {3,-11} | {4,6}", (i + 1), taskList[i].Name, taskList[i].Description, taskList[i].DueDate.ToShortDateString(), taskList[i].Complete));
                }
            }
            PrintCyan("----------------------------------------------------------------------------------------------------------------");

            Console.WriteLine("");
        }
        public static void DisplayTasks(List<Task> taskList)
        {
            //Display entire task list to user

            Console.Clear();

            PrintCyan(String.Format("|  {0,-7} |    {1,-12} | {2,-31} |   {3,-10} | {4,4}", "Index", "\tName", "\t\t\tTask", "Due Date", "Completed"));
            PrintCyan(String.Format("|{0,-6}|{1,-21}|{2,-46}|{3,-4}|{4,4}", "----------", "------------------------", "------------------------------------------------", "--------------", "-----------"));
            for (int i = 0; i < taskList.Count; i++)
            {
                PrintCyan(String.Format("|    {0,-5} |  {1,-21} |  {2,-45} |  {3,-11} | {4,6}", (i + 1), taskList[i].Name, taskList[i].Description, taskList[i].DueDate.ToShortDateString(), taskList[i].Complete));
            }
            PrintCyan("----------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("");
        }
        #endregion

        #region Validation Methods
        public static int ValidateNumberInput(string userInput)
        {
            int validNumber = -1;
            while (!int.TryParse(userInput, out validNumber))
            {
                userInput = GetInput("Invalid input; Please enter a valid number.");

            }

            return validNumber;
        }
        public static DateTime ValidateDateTime(string userInput)
        {
            Regex regex = new Regex("^[0-9]{1,2}[/]{1}[0-9]{1,2}[/]{1}[0-9]{4}$");
            DateTime taskDate;
            while (true)
            {
                while (!regex.IsMatch(userInput))
                {
                    userInput = GetInput("Please input your date in the format of mm/dd/yyyy.");
                    continue;
                }

                while (!DateTime.TryParse(userInput, out taskDate))
                {

                    userInput = GetInput("Please enter the date that the task is due in the format mm/dd/yyyy.");
                    continue;
                }

                break;
            }

            return taskDate;
        }
        public static string ValidateTaskString(string task)
        {
            //Formatting breaks if string is longer than 45 characters.

            while (task.Length > 45)
            {
                task = GetInput("Please input a shorter task description (45 chars or less).");
            }
            return task;
        }
        public static int ValidateIndex(string userInput, List<Task> taskList)
        {
            int inputInt;
            while (true)
            {
                inputInt = ValidateNumberInput(userInput);
                if (inputInt > taskList.Count)
                {
                    userInput = GetInput("Please enter a number that corresponds to a task. There are not that many tasks (yet)!");
                    continue;
                }
                else if (inputInt < 1)
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
        public static int ValidatePropertyIndex(string userInput, List<string> property)
        {
            //Validates correct input for modifying tasks

            int inputInt;
            while (true)
            {
                inputInt = ValidateNumberInput(userInput);
                if (inputInt < 0 || inputInt > property.Count)
                {
                    userInput = GetInput("Please enter a number that corresponds to a property.\nPerson Responsible = 1 , Task Assigned = 2 , Due Date = 3. Press 0 to return to main menu.\n");
                    continue;
                }
                else
                {
                    break;
                }
            }
            return inputInt;
        }
        public static string ValidateName(string inputName, List<string> names)
        {
            for (int i = 0; i < names.Count; i++)
            {
                names[i] = names[i].ToLower();
            }
            while (!names.Contains(inputName.ToLower()))
            {
                inputName = GetInput("Name not found! Please input a name listed above.");
            }
            return inputName;
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
#endregion

    }

}