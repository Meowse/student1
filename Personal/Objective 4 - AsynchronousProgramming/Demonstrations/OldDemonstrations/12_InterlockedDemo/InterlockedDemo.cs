/******************************************************************************
 * 
 *  This program demonstrates how to use the Interlocked class to perform
 *  thread-safe atomic operations without blocking threads.
 * 
 *  The idea behind this synchronization mechanism is to save off the 
 *  original value in a temporary variable, compute a new value and store
 *  that in another new variable, and then compare the original value 
 *  with the temporary saved value. If they're the same, then the current
 *  thread wasn't preempted by another thread and the computation can take
 *  place. Otherwise, another thread came in and made a change and therefore
 *  the current thread cannot complete its task.
 * 
 *****************************************************************************/

using System;
using System.Threading;

namespace InterlockedDemo
{
    class TestClass
    {
        private int _total;

        internal TestClass()
        {
            _total = 1;
        }

        private void RunThread ()
        {
            // Display the total that this thread sees before modifying it.
            // We may see that multiple threads display the same value due to
            // timing issues.
            Console.WriteLine ("{0}: Retrieved total with value of {1}.",
                Thread.CurrentThread.Name, _total);

            // Create a copy of the total value for later comparison. If the
            // _total and origTotal are the same, then no other thread has 
            // modified the value of _total.
            int origTotal = _total;

            // Compute a new total. Using a temporary variable, _total will
            // be set to newTotal if origTotal and _total are the same value.
            int newTotal = origTotal + origTotal;

            // See if any changes occurred between _total and origTotal. If not
            // then it is safe to save the newly computed value in _total.
            int oldTotal = Interlocked.CompareExchange(ref _total, newTotal, origTotal);

            // Display the total with the modification.
            Console.WriteLine ("{0}: Total now contains a value of {1}.",
                Thread.CurrentThread.Name, _total);

            // Simulate a delay in one of the threads.
            if (Thread.CurrentThread.Name == "Thread1")
            {
                Console.WriteLine ("{0}: Sleeping...", Thread.CurrentThread.Name);
                // Sleep for 2.5 seconds.
                Thread.Sleep (2500);
            }
        }

        private int Total
        {
            get { return _total; }
        }

        static void Main()
        {
            // Create an instance of our test class.
            TestClass tc = new TestClass ();

            // Create the first thread object.
            Thread t1 = new Thread(tc.RunThread);

            // Create the second thread object.
            Thread t2 = new Thread(tc.RunThread);

            // Create the third thread object.
            Thread t3 = new Thread(tc.RunThread);

            // Set the names of the threads.
            t1.Name = "Thread1";
            t2.Name = "Thread2";
            t3.Name = "Thread3";

            // Start the three threads. All will access a shared resource
            // and modify data that is stored in the shared resource.
            t1.Start();
            t2.Start();
            t3.Start();

            // Join the threads to ensure they finish.
            t1.Join();
            t2.Join();
            t3.Join();

            // Display the counter in the static class.
            Console.WriteLine ("\nAfter running 3 threads, total should be 8 [ (((1+1)+2)+4) = 8 ].");
            Console.WriteLine ("Total = {0}.", tc.Total);

            // Pause to show results.
            Console.Write ("\nPress <ENTER> to end: ");
            Console.ReadLine();
        }
    }
}
