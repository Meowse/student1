using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace UsingThreads
{
    internal static class SyncObject
    {
        // The actual object that will be used to synchronize.
        private static object _sync;

        // A read-only property that returns the sync object. If the
        // object was not yet created, it is created here.
        public static object Sync
        {
            get
            {
                if (null == _sync)
                {
                    _sync = new object();
                }

                return _sync;
            }
        }
    }
    
    internal class Calculator
    {
        private int _totalValue;
        private Thread _primaryThread;

        public Calculator(Thread primaryThread)
        {
            PrimaryThread = primaryThread;
        }

        // Method called using the ParameterizedThreadStart delegate.
        public void Add(object inputValue)
        {
            try
            {
                int[] inputValues = (int[])inputValue;
                Add(inputValues[0], inputValues[1]);

                // TODO:  The Console window should be synchronized among all threads
                //      Use the Enter method of the Monitor class to synchronize the 
                //      statement below.  Use the SyncObject class to be the controller 
                //      among all threads.  Make sure the Exit method is in a finally block.
                Monitor.Enter(SyncObject.Sync);
                try
                {
                    Console.WriteLine(
                        "\n\tThread {0}: Secondary thread is done.",
                        Thread.CurrentThread.ManagedThreadId);
                }
                finally
                {
                    Monitor.Exit(SyncObject.Sync);
                }
            }
            catch (Exception ex)
            {
                // TODO:  The Console window should be synchronized among all threads
                //      Use the Enter method of the Monitor class to synchronize the 
                //      statement below.  Use the SyncObject class to be the controller 
                //      among all threads.  Make sure the Exit method is in a finally block.
                Monitor.Enter(SyncObject.Sync);
                try
                {
                    Console.WriteLine(
                        "\n\tThread {0}: Exception occurred:\n\t\t{1}",
                        Thread.CurrentThread.ManagedThreadId, ex.Message);
                }
                finally
                {
                    Monitor.Exit(SyncObject.Sync);
                }
            }
        }

        public void Add(int firstNumber, int secondNumber)
        {
            Thread secondaryThread = Thread.CurrentThread;

            // TODO:  The Console window should be synchronized among all threads
            //      Use the Enter method of the Monitor class to synchronize the 
            //      statement below.  Use the SyncObject class to be the controller 
            //      among all threads.  Make sure the Exit method is in a finally block.
            Monitor.Enter(SyncObject.Sync);
            try
            {
                Console.WriteLine("\n\n\tThread {0}: Add method being executed on {1}.\n",
                    secondaryThread.ManagedThreadId, secondaryThread.Name);
            }
            finally
            {
                Monitor.Exit(SyncObject.Sync);
            }

            System.Threading.Thread.Sleep(2000);
            TotalValue = firstNumber + secondNumber;
        }

        public int TotalValue
        {
            get { return _totalValue; }
            private set { _totalValue = value; }
        }

        private Thread PrimaryThread
        {
            get { return _primaryThread; }
            set { _primaryThread = value; }
        }
    }

    internal class Program
    {
        private static void JoinThread(int mainThreadID, Thread secondaryThread)
        {
            // Allow secondary thread to run for the allowed milliseconds before it should
            //      be aborted as something is probably not right.
            if (!secondaryThread.Join(7000))
            {
                Console.WriteLine(
                    "\nThread {0}: Secondary thread {1} - {2} is still alive. calling abort.",
                    mainThreadID, secondaryThread.ManagedThreadId, secondaryThread.Name);
                secondaryThread.Abort();
                // Join the secondary thread again to give time for the CLR to abort the
                //      secondary thread before proceeding.
                if (!secondaryThread.Join(7000))
                {
                    Console.WriteLine("\nThread {0}: Secondary thread {1} - {2} waiting to be aborted.",
                    mainThreadID, secondaryThread.ManagedThreadId, secondaryThread.Name);
                }
            }
        }
        
        private int GetNumericValue()
        {
            bool numericValue = false;
            int valueInt = 0;

            // Loop until input value is numeric.
            while (!numericValue)
            {
                // Enter value to calculate.
                Console.Write("\nEnter numeric value to add: ");

                // Convert input value to a numeric value.
                numericValue = int.TryParse(Console.ReadLine(), out valueInt);
                Console.WriteLine();
                if (numericValue)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Input value not numeric.  Enter again.");
                }
            }

            return valueInt;
        }

        
        static void Main()
        {
            int[] valueInts1= new int[2];
            int[] valueInts2 = new int[2];
            Thread secondaryThread1 = null;
            Thread secondaryThread2 = null;

            Program p = new Program();

            // Declare a thread to reference the current primary thread,
            //      name it, and place its id into a variable.
            Thread primaryThread = Thread.CurrentThread;
            primaryThread.Name = "The Main Thread";
            int threadID = primaryThread.ManagedThreadId;

            Console.WriteLine(
                "\nThread {0}-{1}: Now beginning execution.",
                primaryThread.ManagedThreadId, primaryThread.Name);

            // Create two instances of the Calculator for two separate threads.
            Calculator c1 = new Calculator(primaryThread);
            Calculator c2 = new Calculator(primaryThread);

            // Create an two instances of the ParameterizedThreadStart delegate passing
            //      in the Add method of the Calculator class.
            ParameterizedThreadStart ts1 = new ParameterizedThreadStart(c1.Add);
            ParameterizedThreadStart ts2 = new ParameterizedThreadStart(c2.Add);

            //************************************************************
            //  Secondary thread 1: Create and get values
            
            // Declare and create an instance of the secondary thread
            //      passing in the delegate instance.
            secondaryThread1 = new Thread(ts1);
            // Name the secondary thread.
            secondaryThread1.Name = "The Secondary Thread 1";

            Console.WriteLine(
                "\nThread {0}: Please enter two numbers for the first addition: ", 
                threadID);
            
            // Get first numeric value.
            valueInts1[0] = p.GetNumericValue();

            // Get second numeric value.
            valueInts1[1] = p.GetNumericValue();

            //************************************************************
            //  Secondary thread 2: Create and get values

            // Declare and create an instance of the secondary thread
            //      passing in the delegate instance.
            secondaryThread2 = new Thread(ts2);
            // Name the secondary thread.
            secondaryThread2.Name = "The Secondary Thread 2";

            Console.WriteLine(
                "\nThread {0}: Please enter two numbers for the second addition: ",
                threadID);

            // Get first numeric value.
            valueInts2[0] = p.GetNumericValue();

            // Get second numeric value.
            valueInts2[1] = p.GetNumericValue();

            //*************************************************************
            // Process both secondary threads concurrently.

            //Start the first secondary thread passing in the data object.
            secondaryThread1.Start(valueInts1);

            //Start the second secondary thread passing in the data object.
            secondaryThread2.Start(valueInts2);

            // Join the primary thread to the first secondary thread.
            JoinThread(threadID, secondaryThread1);

            // Join the primary thread to the second secondary thread.
            JoinThread(threadID, secondaryThread2);
            
            Console.WriteLine("\nThread {0}: Total from first addition on thread {1}: {2}",
               primaryThread.ManagedThreadId, secondaryThread1.Name, c1.TotalValue);

            Console.WriteLine("\nThread {0}: Total from second addition on thread {1}: {2}",
                primaryThread.ManagedThreadId, secondaryThread2.Name, c2.TotalValue);

            Console.Write("\nPress any key to end.");
            Console.ReadLine();
        }
    }
}
