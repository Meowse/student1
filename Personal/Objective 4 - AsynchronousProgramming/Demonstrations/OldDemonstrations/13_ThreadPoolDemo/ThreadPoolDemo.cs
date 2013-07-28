/******************************************************************************
 * 
 *  This program demonstrates how to use the thread pool to queue up work
 *  items. It also demonstrates how many threads the pool uses to get all
 *  the work done. Included is a demonstration on using events and signals
 *  to tell the thread pool that a task can start executing.
 * 
 *****************************************************************************/

using System;
using System.Threading;
using System.Collections.Generic;

namespace ThreadPoolDemo
{
    internal static class TaskClass
    {
        // We'll talk about generics later. For now, this is a list of 
        // integers similar to an array.
        private static List<int> _threadIds = new List<int>();

        // The method that will be executed by the thread pool threads when
        // the QueueUserWorkItem() method is called.
        internal static void Compute(Object stateInfo)
        {
            // This code will actually slow down the threads a little due to (a) it's
            // a critical section and (b) it takes time to see if an ID is found in
            // the list.
            lock (_threadIds)
            {
                if (!_threadIds.Contains(Thread.CurrentThread.ManagedThreadId))
                {
                    _threadIds.Add(Thread.CurrentThread.ManagedThreadId);
                }
            }

            // Get the information inside of the object that was passed in.
            int loopValue = (int)stateInfo;

            // Look 'loopValue' times to preform CPU intensive work.
            for (int i = 0; i < loopValue; i++)
            {
                Console.WriteLine("\tThread {0}: Loop iteration = {1}.", Thread.CurrentThread.ManagedThreadId, i);
            }
        }

        // The method that will be executed by the thread pool threads when
        // the RegisterWaitForSingleObject() method is called.
        internal static void Compute(Object stateInfo, bool timedOut)
        {
            // Check to see if we're executing because the handle was signaled,
            // or because the timer popped.
            if (timedOut)
            {
                Console.WriteLine("\n\tHandle timed out - executing.");
            }
            else
            {
                Console.WriteLine("\n\tHandle was signaled - executing.");
            }

            // Call the other Compute() method to actually do the work.
            Compute(stateInfo);
        }

        internal static List<int> ThreadIds
        {
            get { return _threadIds; }
        }
    }

    static class TestClass
    {
        static void Main()
        {
            // Using this set of work items should produce a number of
            // threads that matches the number of CPUs (or cores).
            //ThreadPool.QueueUserWorkItem(TaskClass.Compute, 5);
            //ThreadPool.QueueUserWorkItem(TaskClass.Compute, 10);
            //ThreadPool.QueueUserWorkItem(TaskClass.Compute, 15);
            //ThreadPool.QueueUserWorkItem(TaskClass.Compute, 20);
            //ThreadPool.QueueUserWorkItem(TaskClass.Compute, 25);
            //ThreadPool.QueueUserWorkItem(TaskClass.Compute, 30);
            //ThreadPool.QueueUserWorkItem(TaskClass.Compute, 35);
            //ThreadPool.QueueUserWorkItem(TaskClass.Compute, 40);
            //ThreadPool.QueueUserWorkItem(TaskClass.Compute, 45);
            //ThreadPool.QueueUserWorkItem(TaskClass.Compute, 50);

            // Using this set of work items may produce more threads
            // than the number of CPUs (or cores).
            ThreadPool.QueueUserWorkItem(TaskClass.Compute, 500);
            ThreadPool.QueueUserWorkItem(TaskClass.Compute, 1000);
            ThreadPool.QueueUserWorkItem(TaskClass.Compute, 1500);
            ThreadPool.QueueUserWorkItem(TaskClass.Compute, 2000);
            ThreadPool.QueueUserWorkItem(TaskClass.Compute, 2500);
            ThreadPool.QueueUserWorkItem(TaskClass.Compute, 3000);
            ThreadPool.QueueUserWorkItem(TaskClass.Compute, 3500);
            ThreadPool.QueueUserWorkItem(TaskClass.Compute, 4000);
            ThreadPool.QueueUserWorkItem(TaskClass.Compute, 4500);
            ThreadPool.QueueUserWorkItem(TaskClass.Compute, 5000);

            // Pause to let threads complete.
            Console.Write("\nPress <ENTER> to end: ");
            Console.ReadLine();

            // Display the total number of threads that were created by the pool.
            Console.WriteLine("\nTotal number of threads created by the pool: {0}.", TaskClass.ThreadIds.Count);

            // Now lets use the Compute() method again, but this time
            // have the thread wait until and event occurs. In this case,
            // the thread will be signaled due to a timeout before the 
            // event occurs (calling the Set() method on the AutoResetEvent).
            // The timer timeout value is 2500, but Main will sleep for 3500
            // before raising the event.
            AutoResetEvent myEvent = new AutoResetEvent(false);
            RegisteredWaitHandle myHandle = ThreadPool.RegisterWaitForSingleObject(myEvent, new WaitOrTimerCallback(TaskClass.Compute), 1000, 2500, true);
            Console.WriteLine("Putting MAIN to sleep to allow the remaining thread to run.");
            Thread.Sleep(3500);
            myEvent.Set();

            // Doing this one more time, but in this case the signal 
            // occurs before the timeout. The timer timeout value is 6500,
            // but Main will only sleep for 3500, giving it a chance to
            // raise the event before the timer times out.
            myEvent = new AutoResetEvent(false);
            myHandle = ThreadPool.RegisterWaitForSingleObject(myEvent, new WaitOrTimerCallback(TaskClass.Compute), 1000, 6500, true);
            Console.WriteLine("Putting MAIN to sleep to allow the remaining thread to run.");
            Thread.Sleep(3500);
            myEvent.Set();

            // Pause to show results.
            Console.Write ("\nPress <ENTER> to end: ");
            Console.ReadLine();
        }
    }
}
