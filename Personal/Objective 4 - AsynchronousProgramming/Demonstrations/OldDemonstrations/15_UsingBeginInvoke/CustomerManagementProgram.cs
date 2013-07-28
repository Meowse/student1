/******************************************************************************
 * 
 *  Program: Customer Management Program.
 * 
 *  In this program, we see the first iteration of calling methods 
 *  asynchronously. Rather than calling CustomerDataManager.LoadCustomers()
 *  directly, we create a delegate object based on the delegate that is 
 *  declared in CustomerDataManager. Rather than calling the delegate directly,
 *  which would call the LoadCustomers() method, we use the delegate's
 *  BeginInvoke() method. This method starts a second thread and then returns
 *  control to our main thread.
 * 
 *  Calling EndInvoke() actually pauses the main thread until the second thread 
 *  (LoadCustomers) completed.
 * 
 *  Another way to pause the main thread is to call the AsyncWaitHandle.WaitOne() 
 *  method on the IAsyncResult object (this code is commented out but available 
 *  if you want to use it).
 * 
 *  Finally, check out the delegate declaration in CustomerDataManager. That is 
 *  really all that is required from that class to support asynchronous method
 *  calls. The majority of the work is done from the client side.
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

			// Create an instance of the delegate used to read the
			// data across the slow network.
            CustomerDataManager.DataReader reader = new CustomerDataManager.DataReader(_manager.LoadCustomers);

			// We could have called the delegate directly: reader();. However,
			// we want to do it asynchronously so well use the BeginInvoke()
			// method in the delegate instead. This call will return immediately.
			IAsyncResult iar = reader.BeginInvoke (null, null);

            // To pause the main thread until the delegate call completes, we can
            // use one of two methods: WaitOne or EndInvoke. Calling WaitOne will
            // not supply results if anything is returned from the target method.
            // However, EndInvoke will return results if supplied by the target
            // method. Uncomment the following line to use the WaitOne method
            // rather than EndInvoke to pause the main thread.
//            iar.AsyncWaitHandle.WaitOne();

			// Now we'll wait until the reader is done before going on. Doing this
			// here in this thread makes this look like a synchronous operation
			// because we're blocked until the BeginInvoke() thread ends. But we
            // still have two threads running.
			reader.EndInvoke(iar);

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
