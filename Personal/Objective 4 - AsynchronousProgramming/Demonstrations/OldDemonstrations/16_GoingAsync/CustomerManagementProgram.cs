/******************************************************************************
 * 
 *  Program: Customer Management Program.
 * 
 *  This program demonstrates the use of a callback method that is called when
 *  an asynchronous operation completes. If you haven't noticed yet, adding a 
 *  new customer to the database takes a long time - several seconds. Rather 
 *  that holding up the user interface, let's have the menu reappear immediately
 *  after invoking option #3.
 * 
 *  To begin with, a new delegate was added to the CustomerDataManager class
 *  in Customer.cs. The delegate is called DataAdder. A new delegate was 
 *  required because the signature of the Add method in CustomerDataManager
 *  is different that the signature for LoadCustomers.
 * 
 *  The next step was to add the callback method itself. That method is found
 *  in the UserInterface class in this file and is called AddingDone. It takes
 *  a single argument of type IAsyncResult. Here are the steps that take place
 *  in this callback method:
 * 
 *      1. We cast the IAsyncResult object to an AsyncResult object. 
 *         This allows us to gain access to additional information that 
 *         we couldn't get to through the IAsyncResult reference.
 * 
 *      2. Next, we call the AsyncResult.AsyncDelegate property to obtain 
 *         the delegate that was used to call the Add method asynchronously 
 *         in the BeginInvoke call. 
 * 
 *      3. Using the delegate, we can call EndInvoke and get the boolean 
 *         results of the Add call.
 * 
 *  Finally, we needed to modify the AddNewCustomer method in the UserInterface
 *  class to call Add asynchronously. To do this we performed the following
 *  steps in the AddNewCustomer method:
 * 
 *      1. We created a DataAdder delegate object providing it a reference 
 *         to CustomerDataManager.Add().
 * 
 *      2. Then we created an AsynchCallback delegate and providing a 
 *         reference to our callback method which is named AddingDone. 
 * 
 *      3. Then, using the delegate we call BeginInvoke supplying the 
 *         new customer record, the AsyncCallback delegate and a null 
 *         for the state object.
 * 
 *  Having all this in place now allows us to call the Add method in the 
 *  CustomerDataManager class asynchronously, and get the results of the add
 *  operation via the callback method when the thread ends. 
 * 
 *****************************************************************************/

using System;
using System.Collections;
using System.Runtime.Remoting.Messaging;

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

        // Asynchronous Callback method. This method will be called when
        // an Add record instruction completes. Adding records takes a bit
        // of time over a slow network so we'll perform that action 
        // asynchronously.
        internal void AddingDone(IAsyncResult iar) 
        {
            Console.WriteLine ("\n\n *** AddingDone: Calling EndInvoke().");

            // Cast the IAsyncResult object to an AsyncResult object.
            AsyncResult ar = (AsyncResult)iar;

            // Get the delegate out of the AsyncResult object.
            CustomerDataManager.DataAdder adder = 
                (CustomerDataManager.DataAdder)ar.AsyncDelegate;

            // Call EndInvoke(). Notice that since the Add method in
            // the CustomerDataManager class returns a boolean, we can
            // obtain that value by calling EndInvoke and assigning the
            // result to a boolean variable.
            bool result = adder.EndInvoke(iar);

            // Using the boolean variable, establish a string that represents
            // the results.
            string successString = result == true ? "succeeded" : "failed";

            // Display the results.
            Console.WriteLine ("\n *** AddingDone: Finished. The result of the add is: " + successString + ".\n\n");
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
            // still have two threads running. By simply removing this line of 
            // code, the menu will appear instantaneously while the data is loading.
            // Showing all the records while the data is loading will reveal that
            // a second thread is running.
//			reader.EndInvoke(iar);

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
            // Create a new customer record. This will create a new customer
            // number.
            Customer c = new Customer();

            // Create an instance of the delegate used to read the
            // data across the slow network.
            CustomerDataManager.DataAdder adder = new CustomerDataManager.DataAdder(_manager.Add);

            // Create the delegate that will be used to call
            // the callback method above.
            AsyncCallback addResults = new AsyncCallback(AddingDone);
            
            // We could have called the delegate directly: reader();. However,
            // we want to do it asynchronously so well use the BeginInvoke()
            // method in the delegate instead. This call will return immediately.
            IAsyncResult iar = adder.BeginInvoke(c, addResults, null);
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
