/*****************************************************************************
 * 
 *  This file contains the individual customer class and the collection
 *  class that maintains zero to many customer objects.
 * 
 *****************************************************************************/

using System;
using System.Collections;

namespace CustomerManagementProgram
{
    /// <summary>
    /// This class encapsulates a single customer record.
    /// </summary>
    internal class Customer
    {
        private static Random _custIdRg;    // Used to generate the customer's ID number.
        private static Random _purchasesRg; // Used to generate the # of purchases the customer made.

        private int _customerId;            // Stores the customer's identification number.
        private int _numberOfPurchases;     // Stores the total # of purchases the customer has made.

        // This static constructor will be called once during the lifetime of the 
        // program. It initialized the two Random Number Generators using data from
        // the current system date and time.
        static Customer()
        {
            DateTime dt = DateTime.Now;
            _custIdRg = new Random(dt.Hour + dt.Minute + dt.Second);
            _purchasesRg = new Random(dt.Millisecond);
        }

        // This instance constructor is called when an object of type Customer is 
        // created. It will create the customer ID and the number of purchases. It 
        // also initialized the disposed flag to false.
        internal Customer ()
        {
            CustomerNumber = _custIdRg.Next(10000, 99999);
            PurchaseTotal = _purchasesRg.Next(0, 99);
        }

        // Returns the Customer ID.
        internal int CustomerNumber
        {
            get { return _customerId; }
            set { _customerId = value; }
        }

        // Returns the number of purchases.
        internal int PurchaseTotal
        {    
            get { return _numberOfPurchases; }
            set { _numberOfPurchases = value; }
        }
    }

    /// <summary>
    /// This class simulates the management of multiple customer records and the
    /// various methods that provide access to the list.
    /// </summary>
    internal class CustomerDataManager
    {
        private const int _initialMaxCustomers = 30;    // We'll always start off with this many customers.
        private const int _first = 0;                   // Index of the first customer in the list.
        private int _currentCustomerIndex;              // Index to the current customer record.
        private ArrayList _customerList;                // The dynamic list of customers.

        // This delegate is used to read the data from the database.
		internal delegate void DataReader();

        // This delegate is used to add data to the database.
        internal delegate bool DataAdder(Customer newCustomer);

        internal CustomerDataManager()
        {
            _customerList = new ArrayList();
            _currentCustomerIndex = 0;
        }

        // This method simulates access to a remote database over a slow 
        // network.
        internal void LoadCustomers()
        {
            int i;

            for (i=0; i < _initialMaxCustomers; i++)
            {
                // Simulate slow network access.
                System.Threading.Thread.Sleep (1000);

                // Create and add the customer.
                Customer customer = new Customer();
                _customerList.Add (customer);

                // Shows the user status of obtaining the recordset.
                Console.Write(".");
            }

            Console.WriteLine ("\nLoadCustomers: Loaded {0} customers from the database.", i);
        }

        internal bool Add(Customer newCustomer)
        {
            bool success = true;

            // First, make sure the customer ID isn't already in the list.
            foreach (Customer c in _customerList)
            {
                if (c.CustomerNumber == newCustomer.CustomerNumber)
                {
                    Console.WriteLine ("Add: ERROR - Customer with I.D. {0} already exists.", 
                        newCustomer.CustomerNumber);
                    success = false;
                }
            }

            if (success)
            {
                // Simulate a slow network.
                System.Threading.Thread.Sleep (7500);

                // Add the customer to the list.
                _customerList.Add (newCustomer);

                // Reset the current index so that we're pointing to
                // the top of the list of customers.
                First();
            }

            return success;
        }

        internal Customer Current
        {
            get
            {
                Customer customer = null;

                // Check the value of the current record index.
                if (_currentCustomerIndex < _customerList.Count && _currentCustomerIndex >= 0)
                {
                    // The index was valid so retrieve the customer record.
                    customer = (Customer) _customerList[_currentCustomerIndex];
                }

                return customer;
            }
        }

        internal void MoveNext()
        {
            // Check to see if we're going to go out of bounds.
            if (_currentCustomerIndex < _customerList.Count)
            {
                _currentCustomerIndex++;
            }
            else
            {
                // If we are going to go out of bounds, simulate a circular 
                // queue and set the current index to the first record.
                _currentCustomerIndex = _first;
            }
        }

        internal void First()
        {
            _currentCustomerIndex = _first;
        }

        internal void Last()
        {
            _currentCustomerIndex = _customerList.Count-1;
        }

        internal int Count
        {
            get { return _customerList.Count; }
        }
    }
}
