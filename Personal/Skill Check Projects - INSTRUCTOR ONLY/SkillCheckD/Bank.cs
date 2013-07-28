/* Project Name:   SkillCheckD
 * Developer:        Carol C. Torkko, MCSD - Bellevue College
 * Date:                August, 2011
 * Description:      Convert this application into a multi-threaded application
 *                      while calculating and applying interest to each account
 *                      when the bank class raises the event.  The process needs
 *                      to be synchronized and the event handler can not end
 *                      until all secondary threads are done.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace SkillCheckD
{
    // Interface to be implemented by each derived class of the Account type
    //      EXCEPT for the Account type itself.
    internal interface IAccountInfo
    {
        string PrintClientInformation();
        void CalculateInterest(object interestRateObject);
    }

    // This static class implements the Singleton pattern. This pattern
    // ensures that there is one and only one object throughout the entire
    // application.
    // TODO: 1. Create a singleton object, which means it is guaranteed that only
    //      one instance can be created.  This object is to be used to
    //      synchronize code.  Remember the names you apply here for the class
    //      and any methods as those will be needed by you later on.
    

    
    // Custom Exception class for transactions violating business rules.
    internal class TransactionOutOfRangeException : ApplicationException
    {
        internal TransactionOutOfRangeException() :
            base("Transaction is invalid as the amount is outside of the allowed range.")
        { }

        internal TransactionOutOfRangeException(string messageString) :
            base(messageString)
        { }

        internal TransactionOutOfRangeException(string messageString, Exception ex) :
            base(messageString, ex)
        { }

        internal Account CustomerAccount { get; set; }

        internal decimal TransactionAmount { get; set; }
    }

    // Custom EventArgs class used to store the interest rate.
    internal class InterestRateEventArgs : EventArgs
    {
        decimal _interestRateDecimal;

        public InterestRateEventArgs(decimal interestRateDecimal)
        {
            InterestRate = interestRateDecimal;
        }

        public decimal InterestRate
        {
            get { return _interestRateDecimal; }
            private set { _interestRateDecimal = value; }
        }
    }

    // BankAccounts class
    internal class BankAccounts
    {
        private List<Account> _accounts;

        internal BankAccounts(Bank b, int numberOfAccountsInteger)
        {
            // Creates an instance of collection to contain the bank accounts.
            Accounts = new List<Account>(numberOfAccountsInteger);

            // Subscribes to the bank the event handler to be run when the interest
            //      rate is to be applied.
            b.OnInterestRateCalculate +=
                new Bank.InterestRateCalculateHandler(this.CalculateInterestHandler);
        }

        internal Account this[int accountIdInteger]
        {
            get
            {
                Account account = null;

                foreach (Account acct in Accounts)
                {
                    if (acct.AccountID == accountIdInteger)
                    {
                        account = acct;
                        break;
                    }
                }

                return account;
            }
            set
            {
                // This accessor functions as two different operations. The
                // first is a replace if the account already exists. The 
                // second is an add to the end of the collection if the 
                // account does not exist.
                int indexInteger = Accounts.IndexOf(value);

                if (indexInteger == -1)
                {
                    Accounts.Add(value);
                }
                else
                {
                    Accounts[indexInteger] = value;
                }
            }
        }

        private List<Account> Accounts
        {
            get { return _accounts; }
            set { _accounts = value; }
        }

        internal void PrintAccounts()
        {
            //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
            // Print client information.
            //------------------------------------------------------------------------------------------------------------------------------------------------------------------------

            // Print the information for each client account stored
            //      in the collection.
            foreach (Account account in this.Accounts)
            {
                IAccountInfo ia = account as IAccountInfo;

                if (ia != null)
                {
                    Console.WriteLine(ia.PrintClientInformation());
                }
                else
                {
                    Console.WriteLine("ERROR: Account ID: {0}; Does not implement the IAccountInfo interface.\n",
                        account.AccountID);
                }
            }
        }

        // Called when event is raised by the Bank class to apply interest to each account.
        private void CalculateInterestHandler(object sender, InterestRateEventArgs e)
        {
            // Store each secondary thread in a generic list collection.
            // TODO: 2.  Note the name of the collection below.  This is
            //  to be the collection of secondary foreground threads as they
            //  are started.
            List<Thread> threads = new List<Thread>(Accounts.Count);

            // Store the interest rate passed in through the InterestRateEventArgs into a variable.
            decimal interestRateDecimal = e.InterestRate;

            // Loop through each account to call the CalculateInterest method to calculate
            //      and apply the interest to the balance.
            foreach (Account account in Accounts)
            {
                IAccountInfo ia = account as IAccountInfo;

                if (ia != null)
                {
                    // TODO: 3. Replace the code line below with:
                    //      a.  A call to the same method below on a secondary foreground thread.
                    //      b.  Give the following name to the thread before it is started:
                    //          "ClientName(Account ID: AccountID)"
                    //          The ClientName and AccountID are stored in the account.
                    //      c.  Store the thread created into the threads collection above
                    //          before starting the thread.
                    //      d.  Be sure to pass in the interest rate, which is in the
                    //          "interestRateDecimal" variable.
                    ia.CalculateInterest(interestRateDecimal);
                }
            }

            // Loop through each thread to join to the main thread to make
            //      sure all secondary threads are done before this event
            //      handler is done.
            // TODO: 4. Loop through each thread in the threads collection and join each one
            //  to the main thread that is executing this code:
            //      - Allow for 5 seconds for the join.  
            //      - If the thread is not completed by then, then abort it and join
            //          for another 5 seconds to allow for the abort to take place. 
            //      - A message should be displayed when the thread is being aborted.  
            //      - This message should be synchronized using the "lock" block and 
            //          indicate the name of the thread being aborted.
        
        
        
        }
    }

    // Account class
    internal abstract class Account
    {
        // Class fields
        private int _accountID;
        private string _clientNameString;
        private decimal _balanceDecimal;

        // Class constructor
        internal Account(int accountID, string clientNameString, decimal balanceDecimal)
        {
            ClientName = clientNameString;
            Balance = balanceDecimal;
            AccountID = accountID;
        }

        //Method that updates the balance given a transaction amount.
        internal abstract void UpdateBalance(decimal transactionAmountDecimal);

        // Property that only allows external clients to read.
        public string ClientName
        {
            get { return _clientNameString; }
            private set { _clientNameString = value; }
        }

        // Property that only allows external clients to read.
        public decimal Balance
        {
            get { return _balanceDecimal; }
            protected set { _balanceDecimal = value; }
        }

        // Property that only allows external clients to read.
        public int AccountID
        {
            get { return _accountID; }
            private set { _accountID = value; }
        }
    }

    // Checking class inherits from the abstract Account class and
    //      explicitly implements the IAccountInfo interface. 
    class Checking : Account, IAccountInfo
    {
        private decimal _minBalanceDecimal;

        internal Checking(int accountID, string clientNameString, decimal balanceDecimal
            , decimal minBalanceDecimal) :
            base(accountID, clientNameString, balanceDecimal)
        {
            MinBalance = minBalanceDecimal;
        }

        internal decimal MinBalance
        {
            get { return _minBalanceDecimal; }
            private set { _minBalanceDecimal = value; }
        }

        // An explicit implementation of the PrintClientInformation
        //      method of the IAccountInfo interface.
        string IAccountInfo.PrintClientInformation()
        {
            string clientInfoString = null;

            clientInfoString = "\nAccount Type:\tChecking";
            clientInfoString += "\nClient Account ID:\t" + AccountID.ToString();
            clientInfoString += "\nClient Name:\t" + ClientName;
            clientInfoString += "\nBalance: \t" + Balance.ToString("C");
            clientInfoString += "\nMinimum Balance Allowed: \t" + MinBalance.ToString("C") + "\n";

            return clientInfoString;
        }

        void IAccountInfo.CalculateInterest(object interestRateObject)
        {
            // Set a reference to the current thread.
            // TODO: 5. Set a reference to the thread executing this code so that the
            //      name and id can be included in the Console messages below.
            

            // TODO: 6. Apply exception handling to include ALL remaining code in this method.
            //      The catch block should display a message including the following information:
            //          - Id and name of the thread executing this code.
            //          - A message indicating that the interest rate may or may not have been applied.
            //          - The exception message.
            decimal interestRateDecimal;

            // Convert the object parameter into a decimal data type for calculations.
            if (interestRateObject is decimal)
            {
                interestRateDecimal = (decimal)interestRateObject;
            }
            else
            {
                // TODO: 7. Include two arguments for the WriteLine below that specify the id and
                //  name of the thread executing this code. Id is the first argument and name is
                //  the second argument.
                Console.WriteLine("\n\t{0}-{1}: Interest not applied. Interest rate is invalid.",
                    );
                return;
            }

            // Synchronize the use of the Console AND the balance calculation as those are common
            //      resources among multiple threads.
            // TODO: 8. Synchronize all THREE code lines below together using the "lock" block.
            decimal interestAmountDecimal = Math.Round(Balance * (interestRateDecimal / 100 / 12), 2);
            Balance += interestAmountDecimal;
            // TODO: 9. Complete the code line below to insert two arguments BEFORE the three
            //      already included for the id and name of the thread executing this code.
            Console.WriteLine(
                "\n\t{0}-{1}:Interest amount {2} has been applied at the rate of {3} on {4}.",
                        
                interestAmountDecimal.ToString("C"), (interestRateDecimal / 100).ToString("P"),
                DateTime.Now);
        }

        internal override void UpdateBalance(decimal transactionAmountDecimal)
        {
            decimal balanceDecimal = Balance + transactionAmountDecimal;
            if (balanceDecimal < 0)
            {
                TransactionOutOfRangeException ex = new TransactionOutOfRangeException(
                    "Transaction amount causes an overflow condition. Transaction is rejected.\n");
                ex.CustomerAccount = this;
                ex.TransactionAmount = transactionAmountDecimal;
                throw ex;
            }
            else
            {
                Balance = balanceDecimal;
            }
        }
    }

    // CD class inherits from the abstract Account class and
    //      explicitly implements the IAccountInfo interface. 
    class CD : Account, IAccountInfo
    {
        private DateTime _endDate;
        private readonly decimal _maximumTransactionAmountDecimal;

        internal CD(int accountID, string clientNameString, decimal balanceDecimal
            , DateTime endDate, decimal maxTransAmountDecimal) :
            base(accountID, clientNameString, balanceDecimal)
        {
            EndDate = endDate;
            _maximumTransactionAmountDecimal = maxTransAmountDecimal;
        }

        internal decimal MaximumTransactionAmount
        {
            get
            {
                return _maximumTransactionAmountDecimal;
            }
        }

        internal DateTime EndDate
        {
            get { return _endDate; }
            private set { _endDate = value; }
        }

        // An explicit implementation of the PrintClientInformation
        //      method of the IAccountInfo interface.
        string IAccountInfo.PrintClientInformation()
        {
            string clientInfoString = null;

            clientInfoString = "\nAccount Type:\tCD";
            clientInfoString += "\nClient Account ID:\t" + AccountID.ToString();
            clientInfoString += "\nClient Name:\t" + ClientName;
            clientInfoString += "\nBalance: \t" + Balance.ToString("C");
            clientInfoString += "\nCD End Date: \t" + EndDate.ToString("d") + "\n";

            return clientInfoString;
        }

        internal override void UpdateBalance(decimal transactionAmountDecimal)
        {
            decimal balanceDecimal = Balance + transactionAmountDecimal;
            if ((transactionAmountDecimal < 0) || (transactionAmountDecimal > MaximumTransactionAmount))
            {
                TransactionOutOfRangeException ex = new TransactionOutOfRangeException(
                    "Transaction amount must be greater than 0 and less than or equal to " +
                    MaximumTransactionAmount.ToString("C0") + ". Transaction is rejected.\n");
                ex.CustomerAccount = this;
                ex.TransactionAmount = transactionAmountDecimal;
                throw ex;
            }
            else
            {
                Balance = balanceDecimal;
            }
        }

        void IAccountInfo.CalculateInterest(object interestRateObject)
        {
            // Set a reference to the current thread.
            Thread currentThread = Thread.CurrentThread;

            try
            {
                decimal interestRateDecimal;

                // Convert object value passed in into a decimal data type for calculations.
                if (interestRateObject is decimal)
                {
                    interestRateDecimal = (decimal)interestRateObject;
                }
                else
                {
                    Console.WriteLine("\n\t{0}-{1}: Interest not applied. Interest rate is invalid.",
                        currentThread.ManagedThreadId, currentThread.Name);
                    return;
                }
            
                // Synchronize the use of the Console AND the balance calculation as those are common
                //      resources among multiple threads.
                // TODO: 10. Synchronize all three code lines below together using the Enter method of
                //      the Monitor class.  Be sure to execute the Exit method in a finally block that
                //      belongs only to these 3 lines of code.
                decimal interestAmountDecimal = Math.Round(Balance * (interestRateDecimal / 100 / 12), 2);
                Balance += interestAmountDecimal;
                Console.WriteLine(
                    "\n\t{0}-{1}: Interest amount {2} has been applied at the rate of {3} on {4}.",
                    currentThread.ManagedThreadId, currentThread.Name,
                    interestAmountDecimal.ToString("C"), (interestRateDecimal / 100).ToString("P"),
                    DateTime.Now);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "\n\t{0}-{1}: Interest may or may not have been applied. The following exception occurred:\n{2}",
                    currentThread.ManagedThreadId, currentThread.Name, ex.Message);
            }
        }
    }

    // Bank class implements the IDisposable interface to ensure
    //      that the Dispose method will be called when class is
    //      instantiated in a using block.
    class Bank : IDisposable
    {
        private const string _DATA_FILE_NAME_String = "SkillCheckDAccountData.txt";
        private const string _UPDATE_DATA_FILE_NAME_String = "SkillCheckDAccountUpdate.txt";
        private const decimal _MIN_CD_TRANSACTION_AMOUNT_Decimal = 250.00m;
        private const int _MAX_NUMBER_OF_ACCOUNTS_Integer = 2;

        decimal[] _interestRatesDecimal = {1.5m, 1.1m };

        private static StreamReader _dataFileStreamReader = null;

        // Delegate used by the event.
        internal delegate void InterestRateCalculateHandler (object sender, InterestRateEventArgs e);

        // Event that contains the event handlers as subscribers subscribe to it.
        internal event InterestRateCalculateHandler OnInterestRateCalculate;

        static void Main()
        {
            string inputRecordString = null;
            char[] delimiters = { '\t' };
            string[] fieldsStringArray;
            decimal balanceDecimal = 0.0m;
            decimal minBalanceDecimal = 0.0m;
            decimal amountDecimal = 0.0m;
            DateTime endDate = DateTime.Parse("1/1/0001");
            int accountID = 0;
            int indexInteger = -1;
            Account clientAccount = null;

            try
            {
                // The using block ensures that the Dispose method of the
                //      Bank class will be called when the block is done
                //      executing.
                using (Bank b = new Bank())
                {

                    BankAccounts ba = new BankAccounts(b, _MAX_NUMBER_OF_ACCOUNTS_Integer);

                    #region Read data file to create instances of the two account types and store in the accounts array.

                    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    //  Read data file to create instances of the two account types and store in the accounts array.
                    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                    try
                    //Open file
                    {
                        _dataFileStreamReader = new StreamReader(_DATA_FILE_NAME_String);
                    }
                    catch
                    {
                        Console.WriteLine(
                            "File {0} can not be found. Please locate and store it into the bin/debug folder of this project.",
                            _DATA_FILE_NAME_String);
                        return;
                    }

                    // Loop through file
                    while (_dataFileStreamReader.Peek() != -1)
                    {
                        // Read data, create instance with data
                        inputRecordString = _dataFileStreamReader.ReadLine();
                        fieldsStringArray = inputRecordString.Split(delimiters);

                        if (fieldsStringArray.Length == 1)
                        {
                            // Display a message that the input record is empty and will be bypassed.
                            Console.WriteLine("Record is empty.  It is being bypassed.\n");
                            continue;
                        }
                        else if (fieldsStringArray.Length == 5)
                        {
                            // Make sure first value in input record is a whole numeric value.
                            if (!int.TryParse(fieldsStringArray[0], out accountID))
                            {
                                Console.WriteLine(
                                    "Error: The value of the account ID in the following record is not numeric: {0}\n",
                                    inputRecordString);
                                continue;
                            }

                            // Make sure fourth value in input record is a numeric value.
                            if (!decimal.TryParse(fieldsStringArray[3], out balanceDecimal))
                            {
                                Console.WriteLine("Error: The value of the balance in the following record is not numeric: {0}\n",
                                    inputRecordString);
                                continue;
                            }

                            // Supply a default value if the third value in input record is empty.
                            if (fieldsStringArray[2].Length == 0)
                            {
                                fieldsStringArray[2] = "Unknown";
                            }

                            indexInteger++;

                            // Create an instance of the Account class passing in the client information.
                            switch (fieldsStringArray[1].ToUpper())
                            {
                                case "CHECKING":
                                    // Make sure fifth value in input record is a numeric value.
                                    if (!decimal.TryParse(fieldsStringArray[4], out minBalanceDecimal))
                                    {
                                        Console.WriteLine(
                                            "Error: The value of the minimum balance in the following record is not numeric: {0}\n",
                                            inputRecordString);
                                        break;
                                    }

                                    clientAccount = new Checking(accountID, fieldsStringArray[2], balanceDecimal, minBalanceDecimal);

                                    break;

                                case "CD":
                                    // Make sure fifth value in input record is a date value.
                                    if (!DateTime.TryParse(fieldsStringArray[4], out endDate))
                                    {
                                        Console.WriteLine(
                                            "Error: The value of the end date in the following record is not a date: {0}\n",
                                            inputRecordString);
                                        break;
                                    }

                                    clientAccount = new CD(accountID, fieldsStringArray[2], balanceDecimal, endDate, _MIN_CD_TRANSACTION_AMOUNT_Decimal);
                                    break;

                                default:
                                    Console.WriteLine(
                                        "Error: The second value in the following record is not a recognized account type: {0}\n",
                                         inputRecordString);
                                    break;
                            }

                            if (clientAccount == null)
                            {
                                continue;
                            }
                            else
                            {
                                ba[clientAccount.AccountID] = clientAccount;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error: The following record does not have five values: {0}\n",
                                inputRecordString);
                            continue;
                        }

                        // Remove reference to Account.
                        clientAccount = null;
                    }

                    // Close file
                    _dataFileStreamReader.Close();
                    _dataFileStreamReader = null;

                    #endregion

                    #region Read data file to update the balance in the two account types.

                    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    // Read and process data file containing transaction amounts to update the
                    //      balance in each of the 2 account types.
                    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                    try
                    //Open file
                    {
                        _dataFileStreamReader = new StreamReader(_UPDATE_DATA_FILE_NAME_String);
                    }
                    catch
                    {
                        Console.WriteLine(
                            "File {0} can not be found. Please locate and store it into the bin/debug folder of this project.",
                            _UPDATE_DATA_FILE_NAME_String);
                        return;
                    }

                    // Loop through file
                    while (_dataFileStreamReader.Peek() != -1)
                    {
                        // Read data, create instance with data
                        inputRecordString = _dataFileStreamReader.ReadLine();
                        fieldsStringArray = inputRecordString.Split(delimiters);

                        if (fieldsStringArray.Length == 2)
                        {
                            // Make sure first value in input record is a whole numeric value.
                            if (!int.TryParse(fieldsStringArray[0], out accountID))
                            {
                                Console.WriteLine(
                                    "Error: The value of the account ID in the following record is not numeric: {0}\n",
                                    inputRecordString);
                                continue;
                            }

                            // Make sure second value in input record is a numeric value.
                            if (!decimal.TryParse(fieldsStringArray[1], out amountDecimal))
                            {
                                Console.WriteLine(
                                    "Error: The value of the transaction amount in the following record is not numeric: {0}\n",
                                    inputRecordString);
                                continue;
                            }

                            // Locate the account ID in the accounts array.
                            clientAccount = ba[accountID];

                            // Display error message if account is not found in the array.
                            if (clientAccount == null)
                            {
                                Console.WriteLine(
                                    "Error: There is no account for the account id in this record: {0}\n",
                                    inputRecordString);
                                continue;
                            }

                            try
                            {
                                // Using polymorphism, update the balance in the account from the 
                                //      transaction amount.
                                clientAccount.UpdateBalance(amountDecimal);
                            }
                            catch (TransactionOutOfRangeException ex)
                            {
                                // Because PrintClientInformation is explicitly implemented,
                                //      a cast must occur to execute that method.
                                IAccountInfo ia = ex.CustomerAccount as IAccountInfo;

                                if (ia != null)
                                {
                                    Console.WriteLine("ERROR: {0}\nTransaction amount is: {1}.\nAccount: {2}\n",
                                        ex.Message, ex.TransactionAmount, ia.PrintClientInformation());
                                }
                                else
                                {
                                    Console.WriteLine("ERROR: {0}\nTransaction amount is: {1}.\nAccount: {2}\n",
                                        ex.Message, ex.TransactionAmount, "Customer information not available.");
                                }

                                continue;
                            }

                            ba[clientAccount.AccountID] = clientAccount;
                        }
                        else
                        {
                            Console.WriteLine("Error: The following record does not have two values: {0}\n",
                                inputRecordString);
                            continue;
                        }
                    }  // Loop back up to read and process the next record.

                    // Close file
                    CloseFile();

                    #endregion

                    // Update accounts by adding in interest.  Each element in the array
                    //      represents the interest rate to be applied in each interest period.
                    foreach (decimal interestRateDecimal in b._interestRatesDecimal)
                    {
                        // This is a dummy way to pretend that the bank raises the event at the end of
                        //      every month to apply interest to the accounts.
                        System.Threading.Thread.Sleep(2000);

                        // Raises the event to apply interest to each account.  The custom InterestRateEventArgs
                        //      contains the interest rate amount.
                        InterestRateEventArgs ea = new InterestRateEventArgs(interestRateDecimal);
                        b.OnInterestRateCalculate(b, ea);
                    }

                    //Print the customer account information.
                    ba.PrintAccounts();

                    //Make sure all messages on the console are read before program ends.
                    Console.Write("\nPress any key to end the program.");
                    Console.ReadLine();

                }   // End of using block for Bank class instance.
            }   // End of outer try block in the Main method.
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: The following exception has occurred:\n{0}\nApplication is now ending.",
                    ex.Message);
                CloseFile();
            }
        }   // End of Main method.

        private static void CloseFile()
        {
            if (_dataFileStreamReader != null)
            {
                _dataFileStreamReader.Close();
                _dataFileStreamReader = null;
            }
        }

        // Dispose method is automatically called when the using
        //      block in the Main method ends.  This makes sure that
        //      the file is closed and the destructor is suppressed.
        public void Dispose()
        {
            CloseFile();
            GC.SuppressFinalize(this);
        }

        // Destructor for Bank Class
        ~Bank()
        {
            CloseFile();
            Console.Write("Destructor is running.");
            Console.ReadLine();
        }
    }
}

