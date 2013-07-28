/******************************************************************************
 * 
 *  Notice that this demonstration and the previous one are pretty close
 *  to being the same code. In this case, we use the lock keyword to show
 *  the shorthand method of using monitors.
 * 
 *  NOTE: Exception handling has been eliminated from this code to show
 *        that the compiler does not generate the catch block for the lock
 *        keyword. What this means is that you have to provide exception
 *        handling explicitly either inside of or outside of the lock block.
 * 
 *****************************************************************************/

using System;
using System.Threading;

namespace LockKeyWord
{
    /// <summary>
    /// This class simulates a shared resource.
    /// </summary>
    public class CounterClass
    {
        private static int _counter;

        static CounterClass ()
        {
            Counter = 0;
        }

        public static int Counter
        {
            get { return _counter; }
            set { _counter = value; }
        }

        public static string GetCounter ()
        {
            return (Counter.ToString());
        }
    }

    class TestClass
    {
        public void RunThread ()
        {
            lock(this)
            {
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

                // ***********************************************************************
                //
                // I commented out the following code because there is no handler in this
                // thread for the exception that is being generated below. The design of
                // this method should be revisited to deal with this problem.
                //
                // ***********************************************************************

                //// Throw a fake exception to verify that the lock keyword works. In
                //// this case, no catch block is created by the compiler for the lock
                //// keyword. We will see different behavior when the exception is
                //// thrown.
                //if (Thread.CurrentThread.Name == "Thread2")
                //{
                //    throw new ApplicationException("Fake exception to demonstrate that Exit() works.");
                //}
            }
        }

        static void Main()
        {
            // Create an instance of our test class.
            TestClass tc = new TestClass();

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
