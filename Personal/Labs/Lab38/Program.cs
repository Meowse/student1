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
                Console.WriteLine("\n\n\tThread {0}: Add method being executed on {1}.\n",
                    secondaryThread.ManagedThreadId, secondaryThread.Name);
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
            // Beware that this variable is a common resource among multiple
            //      threads and its update could be interrupted that will cause
            //      this application to not end as the main thread will be
            //      waiting forever to be woke up.
            //TODO:  Use the Interlocked class to synchronize the process of
            //      subtracting one from the _secondaryThreadCount variable.
            //      NOTE: Because this code is outside of the lock, there could
            //      be a time slice break on or more threads in between this code
            //      and the if statement inside the lock. This could cause the
            //      code inside the if block to run multiple times and cause
            //      the wakeup message to appear more than once.  In the real world
            //      this code line should be inside the lock block below.  However,
            //      it is here because there would be no need for the Interlocked
            //      statement to be inside a lock block. The focus of this lab is to
            //      use the Interlocked statement. 
            

            lock (SyncObject.Sync)
            {
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

            Console.WriteLine("\nThread {0}: Total from first addition on thread {1}: {2}",
               primaryThread.ManagedThreadId, secondaryThread1.Name, c1.TotalValue);

            Console.WriteLine("\nThread {0}: Total from second addition on thread {1}: {2}",
                primaryThread.ManagedThreadId, secondaryThread2.Name, c2.TotalValue);

            Console.Write("\nPress any key to end.");
            Console.ReadLine();
        }
    }
}
