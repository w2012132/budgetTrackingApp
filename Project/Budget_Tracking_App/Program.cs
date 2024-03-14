﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget_Tracking_App
{
    class Program
    {
        private static Wallet wallet = new Wallet();
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Main Menu:");
                Console.WriteLine("1. Transaction");
                Console.WriteLine("2. Category");
                Console.WriteLine("3. Budget");
                Console.WriteLine("4. Exit");
                Console.Write("\nSelect an option (1-4): ");

                if (!int.TryParse(Console.ReadLine(), out int mainChoice) || mainChoice < 1 || mainChoice > 4)
                {
                    Console.WriteLine("Invalid choice, please try again.");
                    ContinuePrompt();
                    continue;
                }

                if (mainChoice == 4) break; // Exit the program

                switch (mainChoice)
                {
                    case 1:
                        TransactionSubMenu();
                        break;
                    case 2:
                        CategorySubMenu();
                        break;
                    case 3:
                        BudgetSubMenu();
                        break;
                }
            }
        }

        static void TransactionSubMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Transaction Menu:");
                Console.WriteLine("1. Create ");
                Console.WriteLine("2. Update");
                Console.WriteLine("3. Delete");
                Console.WriteLine("4. View");
                Console.WriteLine("5. Return to Main Menu");
                Console.Write("\nSelect an option (1-5): ");

                if (!int.TryParse(Console.ReadLine(), out int subChoice) || subChoice < 1 || subChoice > 5)
                {
                    Console.WriteLine("Invalid choice, please try again.");
                    ContinuePrompt();
                    continue;
                }

                if (subChoice >= 5) break; // Return to Main Menu
                else if (subChoice == 1)
                { 
                    Console.WriteLine("Transaction create function called");
                }
                else if (subChoice == 2)
                {
                    Console.WriteLine("Transaction Update function called");
                }
                else if (subChoice == 3)
                {
                    Console.WriteLine("Transaction Delete function called");
                }
                else if (subChoice == 4)
                {
                    Console.WriteLine("Transaction View function called");
                }
                ContinuePrompt();
            }
        }
        static void CategorySubMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Category Menu:");
                Console.WriteLine("1. Create");
                Console.WriteLine("2. Rename Category Label");
                Console.WriteLine("3. Delete");
                Console.WriteLine("4. View");
                Console.WriteLine("5. Return to Main Menu");
                Console.Write("\nSelect an option (1-5): ");

                if (!int.TryParse(Console.ReadLine(), out int subChoice) || subChoice < 1 || subChoice > 5)
                {
                    Console.WriteLine("Invalid choice, please try again.");
                    ContinuePrompt();
                    continue;
                }

                if (subChoice >= 5) break; 
                else if (subChoice == 1)
                {
                    //Console.WriteLine("Category create function called");
                    CreateCategoryFromInput();
                }
                else if (subChoice == 2)
                {
                    //Console.WriteLine("Category Rename function called");
                    RenameCategoryFromInput();
                }
                else if (subChoice == 3)
                {
                    //Console.WriteLine("Category Delete function called");
                    RemoveCategoryFromInput();
                }
                else if (subChoice == 4)
                {
                    //Console.WriteLine("Category View function called");
                    DisplayCategoriesFromInput();
                }
                ContinuePrompt();
            }
        }
        static void BudgetSubMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Budget Menu:");
                Console.WriteLine("1. Create");
                Console.WriteLine("2. Delete");
                Console.WriteLine("3. Allocate Budget To Category");
                Console.WriteLine("4. Check Budget");
                Console.WriteLine("5. Return to Main Menu");
                Console.Write("\nSelect an option (1-5): ");

                if (!int.TryParse(Console.ReadLine(), out int subChoice) || subChoice < 1 || subChoice > 5)
                {
                    Console.WriteLine("Invalid choice, please try again.");
                    ContinuePrompt();
                    continue;
                }

                if (subChoice >= 5) break; // Return to Main Menu
                else if (subChoice == 1)
                {
                    //Console.WriteLine("Budget create function called");
                    CreateBudgetFromInput();
                }
                else if (subChoice == 2)
                {
                    //Console.WriteLine("Budget Delete function called");
                    RemoveBudgetFromInput();
                    
                }
                else if (subChoice == 3)
                {
                    //Console.WriteLine("Allocate Budget To Category function called");
                    AllocateBudgetToCategoryFromInput();
                }
                else if (subChoice == 4)
                {
                    //Console.WriteLine("Budget View function called");
                    ViewBudgetByDate();
                }

                ContinuePrompt();
            }
        }

        static void ContinuePrompt()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        #region Category
        static void CreateCategoryFromInput()
        {
            Console.WriteLine("Enter category label:");
            string label = Console.ReadLine();

            Console.WriteLine("Enter budget allocated for this category:");
            string budgetInput = Console.ReadLine();
            if (!double.TryParse(budgetInput, out double budgetAllocated))
            {
                Console.WriteLine("Invalid budget format. Please enter a valid number.");
                return;
            }

            Console.WriteLine("Enter month and year for the category (format MM/yyyy):");
            string monthYearInput = Console.ReadLine();

            if (DateTime.TryParseExact(monthYearInput, "MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime monthYear))
            {
                Category newCategory = new Category(label, budgetAllocated, monthYear);
                if (wallet.CreateCategory(newCategory))
                {
                    Console.WriteLine("Category created successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to create category.");
                }
            }
            else
            {
                Console.WriteLine("Invalid date format.");
            }
        }
        static void RenameCategoryFromInput()
        {
            DateTime currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); // This sets the time to the first day of the current month

            Console.WriteLine("Current month's categories:");
            wallet.DisplayCategories(currentDate); // Assumes DisplayCategories method is updated to accept DateTime

            // Ask for the old label
            Console.WriteLine("Enter the label of the category you wish to rename:");
            string oldLabel = Console.ReadLine();

            // Ask for the new label
            Console.WriteLine("Enter the new label for the category:");
            string newLabel = Console.ReadLine();

            // Perform the renaming
            if (wallet.RenameCategory(oldLabel, newLabel, currentDate))
            {
                Console.WriteLine("Category renamed successfully.");
            }
            else
            {
                Console.WriteLine("Failed to rename category. Make sure it exists and you entered the correct date.");
            }
        }
        public static void RemoveCategoryFromInput()
        {
            Console.WriteLine("Enter the label of the category you want to remove:");
            string label = Console.ReadLine();

            Console.WriteLine("Enter the month and year of the category to remove (format MM/yyyy):");
            string monthYearInput = Console.ReadLine();

            if (!DateTime.TryParseExact(monthYearInput, "MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime monthYear))
            {
                Console.WriteLine("Invalid date format. Please use MM/yyyy format.");
                return;
            }

            // Assuming 'wallet' is your static Wallet instance accessible from here
            bool isRemoved = wallet.RemoveCategory(label, monthYear);
            if (isRemoved)
            {
                Console.WriteLine("Category successfully removed.");
            }
            else
            {
                Console.WriteLine("Category not found or could not be removed.");
            }
        }
        public static void DisplayCategoriesFromInput()
        {
            Console.WriteLine("Enter the month and year to display categories (format MM/yyyy):");
            string monthYearInput = Console.ReadLine();

            if (!DateTime.TryParseExact(monthYearInput, "MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime monthYear))
            {
                Console.WriteLine("Invalid date format. Please use MM/yyyy format.");
                return;
            }
            wallet.DisplayCategories(monthYear);
        }

        #endregion

        #region Budget
        static void CreateBudgetFromInput()
        {
            Console.WriteLine("Enter the total sum to allocate for the budget:");
            string sumInput = Console.ReadLine();
            if (!double.TryParse(sumInput, out double sumToAllocate))
            {
                Console.WriteLine("Invalid sum format. Please enter a valid number.");
                return;
            }

            Console.WriteLine("Enter month and year for the budget (format MM/yyyy):");
            string monthYearInput = Console.ReadLine();
            if (DateTime.TryParseExact(monthYearInput, "MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime monthYear))
            {
                Budget newBudget = new Budget(sumToAllocate, monthYear);
                if (wallet.AddBudget(newBudget))
                {
                    Console.WriteLine("Budget added successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to add budget.");
                }
            }
            else
            {
                Console.WriteLine("Invalid date format. Please follow the MM/yyyy format.");
            }
        }
        static void RemoveBudgetFromInput()
        {
            Console.WriteLine("Enter month and year of the budget to remove (format MM/yyyy):");
            string monthYearInput = Console.ReadLine();
            if (DateTime.TryParseExact(monthYearInput, "MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime monthYear))
            {
                Budget budgetToRemove = wallet.GetBudgetByDate(monthYear);
                if (budgetToRemove != null)
                {
                    if (wallet.RemoveBudget(budgetToRemove))
                    {
                        Console.WriteLine("Budget removed successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to remove budget.");
                    }
                }
                else
                {
                    Console.WriteLine($"No budget found for {monthYear.ToString("MM/yyyy")}");
                }
            }
            else
            {
                Console.WriteLine("Invalid date format. Please follow the MM/yyyy format.");
            }
        }
        static void AllocateBudgetToCategoryFromInput()
        {
            Console.WriteLine("Enter the label of the category to allocate budget to:");
            string categoryLabel = Console.ReadLine();

            Console.WriteLine("Enter the amount to allocate:");
            if (!double.TryParse(Console.ReadLine(), out double amount))
            {
                Console.WriteLine("Invalid amount format. Please enter a valid number.");
                return;
            }

            Console.WriteLine("Enter the month and year for the category (format MM/yyyy):");
            string monthYearInput = Console.ReadLine();
            if (!DateTime.TryParseExact(monthYearInput, "MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime monthYear))
            {
                Console.WriteLine("Invalid date format. Please follow the MM/yyyy format.");
                return;
            }

            if (wallet.AllocateBudgetToCategory(categoryLabel, amount, monthYear))
            {
                Console.WriteLine($"Budget of {amount} allocated to category '{categoryLabel}' for {monthYear.ToString("MM/yyyy")}.");
            }
            else
            {
                Console.WriteLine("Failed to allocate budget. Make sure the category exists and the date is correct.");
            }
        }
        static void ViewBudgetByDate()
        {
            Console.WriteLine("Enter the month and year to view the budget (format MM/yyyy):");
            string monthYearInput = Console.ReadLine();
            DateTime monthYear;
            if (!DateTime.TryParseExact(monthYearInput, "MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out monthYear))
            {
                Console.WriteLine("Invalid date format. Please follow the MM/yyyy format.");
                return;
            }

            Budget budget = wallet.GetBudgetByDate(monthYear);
            if (budget != null)
            {
                Console.WriteLine($"Budget for {monthYear.ToString("MM/yyyy")}:");
                Console.WriteLine($"- Amount allocated: {budget.GetBudget()}");
                Console.WriteLine($"- Remaining budget: {budget.GetremainingBudget()}");
            }
            else
            {
                Console.WriteLine("No budget found for the specified month and year.");
            }
        }

        #endregion
    }

}
