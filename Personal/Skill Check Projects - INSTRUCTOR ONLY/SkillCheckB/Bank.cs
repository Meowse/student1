/* Project Name:   SkillCheckB
 * Developer:        Carol C. Torkko, MCSD - Bellevue College
 * Date:                July, 2011
 * Description:      Create and implement an interface.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SkillCheckB
{
    // TODO: Declare an interface called IAccountInfo with
    //      one member that returns a string with a name
    //      PrintClientInformation.

    

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

    // BankAccounts class
    internal class BankAccounts
    {
        private List<Account> _accounts;

        internal BankAccounts(int numberOfAccountsInteger)
        {
            Accounts = new List<Account>(numberOfAccountsInteger);
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

            // TODO: Complete the argument in the Console WriteLine method
            //      in the foreach below to display the output of the PrintClientInformation method
            //      of "account".  Beware that the PrintClientInformation method
            //      is an explicit implementation of the IAccountInfo interface.  A cast will
            //      be required.  It would be best to NOT complete this TODO until all
            //      TODOs in the Checking and CD classes are complete.
            foreach (Account account in this.Accounts)
            {
                    Console.WriteLine(.PrintClientInformation());
            }
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

        // Method that formats the client information and returns a formatted string.
        // TODO: Remove the abstract method below from the Account class.
        internal abstract string PrintClientInformation();

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

    // TODO: Explicitly implement the IAccountInfo interface in the
    //      Checking class below. 
    class Checking : Account
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

        // TODO: Wrap the commented block of code below in an
        //      explicit implementation of the IAccountInfo interface
        //      method named PrintClientInformation.  Then uncomment
        //      the block of code.
        
        
            //string clientInfoString = null;

            //clientInfoString = "\nAccount Type:\tChecking";
            //clientInfoString += "\nClient Account ID:\t" + AccountID.ToString();
            //clientInfoString += "\nClient Name:\t" + ClientName;
            //clientInfoString += "\nBalance: \t" + Balance.ToString("C");
            //clientInfoString += "\nMinimum Balance Allowed: \t" + MinBalance.ToString("C") + "\n";

            //return clientInfoString;
        

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

    // TODO: Explicitly implement the IAccountInfo interface in the
    //      CD class below. 
    class CD : Account
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

        // TODO: Wrap the commented block of code below in an
        //      explicit implementation of the IAccountInfo interface
        //      method named PrintClientInformation.  Then uncomment
        //      the block of code.
        
        
            //string clientInfoString = null;

            //clientInfoString = "\nAccount Type:\tCD";
            //clientInfoString += "\nClient Account ID:\t" + AccountID.ToString();
            //clientInfoString += "\nClient Name:\t" + ClientName;
            //clientInfoString += "\nBalance: \t" + Balance.ToString("C");
            //clientInfoString += "\nCD End Date: \t" + EndDate.ToString("d") + "\n";

            //return clientInfoString;
        

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
    }

    // Bank class
    // TODO: Implement the IDisposable interface in the Bank class below
    //      to make sure both outside files are closed before the application ends.
    class Bank 
    {
        private const string _DATA_FILE_NAME_String = "SkillCheckBAccountData.txt";
        private const string _UPDATE_DATA_FILE_NAME_String = "SkillCheckBAccountUpdate.txt";
        private const decimal _MIN_CD_TRANSACTION_AMOUNT_Decimal = 250.00m;
        private const int _MAX_NUMBER_OF_ACCOUNTS_Integer = 2;

        private static StreamReader _dataFileStreamReader = null;

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
                // TODO: Declare and create an instance of the Bank class within
                //      a using block that includes ALL the remaining code of this
                //      outer try block.  There is a TODO at the location where the
                //      using block ends.
                
                

                    BankAccounts ba = new BankAccounts(_MAX_NUMBER_OF_ACCOUNTS_Integer);

                    #region  Read data file to create instances of the two account types and store in the accounts array.

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
                                // TODO: Complete the third argument in the Console WriteLines method
                                //      below to display the output of the PrintClientInformation method
                                //      of ex.CustomerAccount.  Beware that the PrintClientInformation method
                                //      is an explicit implementation of the IAccountInfo interface.  A cast will
                                //      be required.
                                    Console.WriteLine("ERROR: {0}\nTransaction amount is: {1}.\nAccount: {2}\n",
                                        ex.Message, ex.TransactionAmount, );

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

                    //Print the customer account information.
                    ba.PrintAccounts();

                // TODO: Close off the using block here.

                
                //Make sure all messages on the console are read before program ends.
                Console.Write("Press any key to end the program.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: The following exception has occurred:\n{0}\nApplication is now ending.",
                    ex.Message);
                CloseFile();

                //Make sure all messages on the console are read before program ends.
                Console.Write("Press any key to end the program.");
                Console.ReadLine();
            }
        }

        private static void CloseFile()
        {
            if (_dataFileStreamReader != null)
            {
                _dataFileStreamReader.Close();
                _dataFileStreamReader = null;
            }
        }

        // TODO: Implement the Dispose method below to meet the requirement for
        //      the IDisposable interface.  Execute the CloseFile method to ensure
        //      that the files are closed and then suppress the destructor from
        //      running through the Garbage Collector.



        // Destructor for Bank Class
        // TODO: Implement the Destructor. Include the commented block of code of 5
        //          lines below in the Destructor. Then uncomment the block of code.
        
        
            //CloseFile();
            //Console.Write("Destructor is running.");

            ////Make sure all messages on the console are read before program ends.
            //Console.Write("Press any key to end the program.");
            //Console.ReadLine();
        
    }
}

