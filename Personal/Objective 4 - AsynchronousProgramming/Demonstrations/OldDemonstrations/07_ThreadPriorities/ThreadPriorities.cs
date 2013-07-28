/*****************************************************************************
 * 
 *  This sample demonstrates the order in which Windows schedules threads
 *  based on their priority settings. The results may vary depending on how
 *  many processors the machine has.
 * 
 *****************************************************************************/

using System;
using System.Threading;

internal class ThreadedClass
{
    private string _threadName;

    internal ThreadedClass(string threadName)
    {
        ThreadName = threadName;
    }

    internal void ThreadMethod()
    {
        Console.WriteLine("\n{0}: Started!\n", ThreadName);

        // Loop 100,000,000 times
        for (int i=0; i < 100000000; i++)
        {
            // Display a message every 10,000,000 times.
            if (i%10000000 == 0)
            {
                Console.WriteLine ("\t{0}: 'i' has a value of {1}. Time: {2}.",
                    ThreadName,
                    i,
                    DateTime.Now.ToString("T"));
            }
        }

        Console.WriteLine("\n *** {0}: Thread ending. Priority: {1}, Time: {2}.\n", 
            ThreadName,
            Thread.CurrentThread.Priority,
            DateTime.Now.ToString("T"));
    }

    private string ThreadName
    {
        get { return _threadName; }
        set { _threadName = value; }
    }
}

class TestClass
{
    static void Main()
    {
        // Create 6 ThreadedClass objects.
        ThreadedClass tc1 = new ThreadedClass("Thread1");
        ThreadedClass tc2 = new ThreadedClass("Thread2");
        ThreadedClass tc3 = new ThreadedClass("Thread3");
        ThreadedClass tc4 = new ThreadedClass("Thread4");
        ThreadedClass tc5 = new ThreadedClass("Thread5");
        ThreadedClass tc6 = new ThreadedClass("Thread6");

        Console.WriteLine ("Main - Creating worker threads.");

        // Create the 6 threads.
        Thread t1 = new Thread(tc1.ThreadMethod);
        Thread t2 = new Thread(tc2.ThreadMethod);
        Thread t3 = new Thread(tc3.ThreadMethod);
        Thread t4 = new Thread(tc4.ThreadMethod);
        Thread t5 = new Thread(tc5.ThreadMethod);
        Thread t6 = new Thread(tc6.ThreadMethod);

        // Set the priority of each thread.
        t1.Priority = ThreadPriority.Lowest;
        t2.Priority = ThreadPriority.Normal;
        t3.Priority = ThreadPriority.BelowNormal;
        t4.Priority = ThreadPriority.Highest;
        t5.Priority = ThreadPriority.AboveNormal;
        t6.Priority = ThreadPriority.Lowest;

        Console.WriteLine ("Expect the threads to finish in the following order:");
        Console.WriteLine ("\tThread4");
        Console.WriteLine ("\tThread5");
        Console.WriteLine ("\tThread2");
        Console.WriteLine ("\tThread3");
        Console.WriteLine ("\tThread1 & Thread6 (order will vary)\n");

        // Start the threads in order. Note the order of how
        // the threads are started. Because of their priority
        // relative to each other, we can expect #4 to end 
        // first, followed by #5, #2, #3, and #1/#6 since they
        // have the same priority.
        t1.Start();
        t2.Start();
        t3.Start();
        t4.Start();
        t5.Start();
        t6.Start();

        Console.WriteLine ("\nMain - Worker threads started.\n");

        // Join the last thread that was started. Give the program
        // two minutes to run before raising an error.
        if (!t6.Join(120000))
        {
            Console.WriteLine ("\nMain - At least one thread is STILL running.");
        }

        // Pause the program.
        Console.Write ("\nPress <ENTER> to end: ");
        Console.ReadLine();
    }
}

