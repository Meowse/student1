/*****************************************************************************
 * 
 *  This sample demonstrates the use of the Monitor class and how to set
 *  up a critical section. It also demonstrates putting the Exit() call
 *  in the finally block to ensure that a thread doesn't remain permanently
 *  locked in the event of an exception.
 * 
 *****************************************************************************/
using System;
using System.Threading;

namespace ThreadMonitors
{
    /// <summary>
    /// This class simulates a shared resource.
    /// </summary>
    internal class CounterClass
    {
        private static int _counter;

        static CounterClass ()
        {
            Counter = 0;
        }

        // This property is a critical section of
        // code because the value of the counter
        // can be changed via the set accessor.
        internal static int Counter
        {
            get { return _counter; }
            set { _counter = value; }
        }

        internal static string GetCounter()
        {
            return (Counter.ToString());
        }
    }

    class TestClass
    {
        private void RunThread()
        {
            try
            {
                Console.WriteLine ("{0}: Attempting to enter critical section.", 
                    Thread.CurrentThread.Name);

                // Lock the following critical section.
                Monitor.Enter(this);

                // Get a local copy of the data stored in the shared resource.
                int myCounter = CounterClass.Counter;

                // Display the counter that this thread sees before modifying it.
                Console.WriteLine ("{0}: Retrieved counter with value of {1}.", 
                    Thread.CurrentThread.Name, myCounter);

                // Modify the value stored in the local copy.
                myCounter++;

                // Display the counter with the modification.
                Console.WriteLine ("{0}: Counter now contains a value of {1}.", 
                    Thread.CurrentThread.Name, myCounter);

                // Simulate a delay in one of the threads. In this case, because the
                // other thread is blocked by the Monitor.Enter(this); call, this 
                // delay just delays the unlocking of the critical section.
                if (Thread.CurrentThread.Name == "Thread1")
                {
                    Console.WriteLine ("{0}: Sleeping...", Thread.CurrentThread.Name);
                    // Sleep for 2.5 seconds.
                    Thread.Sleep (2500);
                }

                // Write value of counter back to resource.
                Console.WriteLine ("{0}: Counter value {1} written back to resource.", 
                    Thread.CurrentThread.Name, myCounter);

                // Write the value of the local copy back out to the shared resource.
                CounterClass.Counter = myCounter;

                // Throw a fake exception to verify that Exit() works.
                if (Thread.CurrentThread.Name == "Thread2")
                {
                    throw new ApplicationException("Fake exception to demonstrate that Exit() works.");
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine ("\nEXCEPTION {0}", exc.Message);
            }
            finally
            {
                Console.WriteLine ("{0}: Exiting the critical section.", 
                    Thread.CurrentThread.Name);

                // Unlock the critical section.
                Monitor.Exit(this);
            }
        }

        static void Main()
        {
            // Create an instance of our test class.
            TestClass tc = new TestClass ();

            // Create the first thread object.
            Thread t1 = new Thread(tc.RunThread);

            // Create the second thread object.
            Thread t2 = new Thread(tc.RunThread);

            // Set the names of the threads.
            t1.Name = "Thread1";
            t2.Name = "Thread2";

            // Start the two threads. Both will access a shared resource
            // and modify data that is stored in the shared resource.
            t1.Start();
            t2.Start();

            // Join the threads to ensure they finish.
            t1.Join();
            t2.Join();

            // Display the counter in the static class.
            Console.WriteLine ("\nAfter running 2 threads, counter should be 2.");
            Console.WriteLine ("Counter = {0}.", CounterClass.Counter);

            // Pause to show results.
            Console.Write ("\nPress <ENTER> to end: ");
            Console.ReadLine();
        }
    }
}
