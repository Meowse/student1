/******************************************************************************
 * 
 *  Program: Customer Management Program.
 * 
 *  This program demonstrates the bad performance an application can reveal
 *  when accessing data over a slow network. The slow network is simulated by
 *  Thread.Sleep() calls. There are two main areas where the program will
 *  slow down, reading the initial data and adding a new record.
 * 
 *  Check out Customer.cs. There are two classes in that file, Customer and 
 *  CustomerDataManager. Customer is simply a customer record. The 
 *  CustomerDataManager class manages multiple Customer records and accessing
 *  the data store.
 * 
 *****************************************************************************/

using System;
using System.Collections;

namespace CustomerManagementProgram
{
    /// <summary>
    /// This class contains all the user interface code. It also works with the
    /// CustomerDataManager class to gain access to customers.
    /// </summary>
    internal class UserInterface
    {
        CustomerDataManager _manager;

        internal UserInterface()
        {
            _manager = new CustomerDataManager();
        }

        internal void Run()
        {
            // Boolean that tells us when to stop running.
            bool keepRunning = true;

            // Load all the customer data from the "database".
            Console.Write ("Loading customer data.");
            _manager.LoadCustomers();
            Console.WriteLine ("");

            // Loop until the user ends.
            while (keepRunning)
            {
                DisplayMenu();
                string sChoice = Console.ReadLine();

                // Determine what the user wants to do.
                switch (sChoice)
                {
                    case "0":   // Exit the program
                        keepRunning = false;
                        break;

                    case "1":   // Display the current customer record.
                        DisplayCustomer();
                        Console.WriteLine();
                        break;

                    case "2":   // Display ALL the customer records.
                        for (int i = 0; i < _manager.Count; i++)
                        {
                            Console.Write ("{0}) ", i+1);
                            DisplayCustomer();
                            Console.WriteLine();
                        }

                        // Since we walked through the whole list, reset the current index.
                        _manager.First();
                        break;

                    case "3":   // Add a new customer record.
                        AddNewCustomer();
                        break;

                    case "4":   // Show a count of the number of customers in the list.
                        Console.WriteLine("Total number of customers is {0}.", _manager.Count);
                        break;

                    default:
                        Console.WriteLine ("Run: ERROR - {0} is not a valid menu option.", sChoice);
                        break;
                }
            }
        }

        private void DisplayMenu()
        {
            Console.WriteLine ("\nMain Menu\n");
            Console.WriteLine ("    1. Retrieve Next Customer And Display.");
            Console.WriteLine ("    2. Display ALL Customers.");
            Console.WriteLine ("    3. Add New Customer.");
            Console.WriteLine ("    4. Get Current Customer Count.");
            Console.WriteLine ("    0. Exit");
            Console.Write ("\nEnter Your Choice: ");
        }

        private void DisplayCustomer ()
        {
            Customer c = _manager.Current;
            if (null != c)
            {
                Console.Write ("ID: {0}, Total Purchases: {1}", c.CustomerNumber, c.PurchaseTotal);
                _manager.MoveNext();
            }
        }

        private void AddNewCustomer()
        {
            Customer c = new Customer();

            if (!_manager.Add(c))
            {
                Console.WriteLine ("Run: ERROR - Could not add customer {0}.", c.CustomerNumber);
            }
            else
            {
                Console.WriteLine ("Run: Customer {0} was successfully added.", c.CustomerNumber);
            }
        }
    }

    /// <summary>
    /// This class just contains the Main() method, which is our entry point into this program.
    /// </summary>
	class Tester
	{
		static void Main()
		{
			UserInterface ui = new UserInterface();
            ui.Run();
		}
	}
}
