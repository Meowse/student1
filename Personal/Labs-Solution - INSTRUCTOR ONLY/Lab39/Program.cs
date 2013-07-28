using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace UsingThreads
{
    // This delegate will be used by each thread to call the callback
    // method in the Program class.
    internal delegate void CallbackDelegate();

    // This static class implements the Singleton pattern. This pattern
    // ensures that there is one and only one object throughout the entire
    // application.
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
        private static int threadCount = 0;

        // Member variable that stores the CallbackDelegate.
        CallbackDelegate _completedMethod;

        
        public Calculator(Thread primaryThread, CallbackDelegate callback)
        {
            PrimaryThread = primaryThread;
            Callback = callback;
        }

        // Method called using the ParameterizedThreadStart delegate.
        public void Add(object inputValue)
        {
            try
            {
                int[] inputValues = (int[])inputValue;
                Add(inputValues[0], inputValues[1]);

                // Using the lock block to synchronize the common resource, which
                //      is the Console window.
                lock (SyncObject.Sync)
                {
                    Console.WriteLine("\n\tThread {0}: Secondary thread is done.",
                        Thread.CurrentThread.ManagedThreadId);
                }
            }
            catch (Exception ex)
            {
                // Using the lock block to synchronize the common resource, which
                //      is the Console window.
                lock (SyncObject.Sync)
                {
                    Console.WriteLine(
                        "\n\tThread {0}: Exception occurred:\n\t\t{1}",
                        Thread.CurrentThread.ManagedThreadId, ex.Message);
                }
            }
            finally
            {
                if (Callback != null)
                {
                    Callback();
                }
            }
        }

        public void Add(int firstNumber, int secondNumber)
        {
            Thread secondaryThread = Thread.CurrentThread;

            // Using the lock block to synchronize the common resource, which
            //      is the Console window.
            lock (SyncObject.Sync)
            {
                Console.WriteLine("\n\n\tThread {0}: Add method being executed.\n",
                    secondaryThread.ManagedThreadId);
            }

            System.Threading.Thread.Sleep(4000);
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

        // This property is used to access the callback delegate
        // object. This is private because it's only used in this 
        // class
        private CallbackDelegate Callback
        {
            get { return _completedMethod; }
            set { _completedMethod = value; }
        }
    }

    internal class Program
    {
        // This variable indicates how many secondary threads will be
        // started.  After each secondary thread is done, the callback
        //  method is called and one is subtracted from this variable.
        //  When this variable reaches zero, the primary thread will be
        //  signaled to start up again.
        private static int _secondaryThreadCount = 2;

        // Create an AutoResetEvent object to be used to synchronize
        // the primary thread and all the secondary threads. The
        // boolean indicates that this object IS NOT in a signaled
        // state.
        private static AutoResetEvent _signalPrimaryThread =
            new AutoResetEvent(false);
        
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

        // This callback method is called by each thread to indicate they are done.
        public static void ThreadDoneCallback()
        {
            lock (SyncObject.Sync)
            {
                // Synchronizing the update of this variable ensures that the update
                //      will not be interrupted and adds insurance that the main thread
                //      will not wait forever to wakeup.
                Interlocked.Decrement(ref _secondaryThreadCount);

                // If all the threads completed, it's time to wake up the
                // primary thread.
                if (_secondaryThreadCount == 0)
                {
                    Console.WriteLine
                        ("\n\t{0}: Signaling the primary thread to wake up."
                            , Thread.CurrentThread.ManagedThreadId);

                    // Use the AutoResetEvent object to signal the primary thread.
                    _signalPrimaryThread.Set();
                }
            }
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

            //Declare and create an instance of the callback delegate.
            CallbackDelegate cd =
                new CallbackDelegate(Program.ThreadDoneCallback);

            // Create two instances of the Calculator for two separate threads.
            Calculator c1 = new Calculator(primaryThread, cd);
            Calculator c2 = new Calculator(primaryThread, cd);

            //************************************************************
            //  Get values for first background thread.

            Console.WriteLine(
                "\nThread {0}: Please enter two numbers for the first addition: ", 
                threadID);
            
            // Get first numeric value.
            valueInts1[0] = p.GetNumericValue();

            // Get second numeric value.
            valueInts1[1] = p.GetNumericValue();

            //************************************************************
            //  Secondary thread 2: Create and get values

            Console.WriteLine(
                "\nThread {0}: Please enter two numbers for the second addition: ",
                threadID);

            // Get first numeric value.
            valueInts2[0] = p.GetNumericValue();

            // Get second numeric value.
            valueInts2[1] = p.GetNumericValue();

            //*************************************************************
            // Process both secondary background threads concurrently.

            //Start the first secondary background thread passing in the data object.
            // TODO: Start a background thread to execute the Add method using the c1
            //  reference and passing in the valueInts1 array.  Give an error message
            //  if the queuing of the background thread is not successful.
            if (!ThreadPool.QueueUserWorkItem(c1.Add, valueInts1))
            {
                Console.WriteLine("Error in queuing thread to execute c1.Add with valueInts1");
            }


            //Start the second secondary background thread passing in the data object.
            // TODO: Start a background thread to execute the Add method using the c2
            //  reference and passing in the valueInts2 array.  Give an error message
            //  if the queuing of the background thread is not successful.
            if (!ThreadPool.QueueUserWorkItem(c2.Add, valueInts2))
            {
                Console.WriteLine("Error in queuing thread to execute c2.Add with valueInts2");
            }
            
            
            lock (SyncObject.Sync)
            {
                Console.WriteLine
                    ("\n{0}: Waiting for threads to complete.",
                    threadID);
            }

            // Put the primary thread into a wait state. This
            // will wait until the AutoResetEvent object is 
            // signaled through a call to Set().
            _signalPrimaryThread.WaitOne();

            Console.WriteLine("\nThread {0}: Total from first addition: {1}",
               primaryThread.ManagedThreadId, c1.TotalValue);

            Console.WriteLine("\nThread {0}: Total from second addition: {1}",
                primaryThread.ManagedThreadId, c2.TotalValue);

            Console.Write("\nPress any key to end.");
            Console.ReadLine();
        }
    }
}
