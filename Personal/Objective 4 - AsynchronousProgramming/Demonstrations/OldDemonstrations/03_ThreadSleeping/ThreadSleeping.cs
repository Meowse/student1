/*****************************************************************************
 * 
 *  This program demonstrates how to put a thread to sleep and then interrupt
 *  it later. As part of interrupting a sleeping thread, an exception will
 *  be raised which needs to be handled.
 * 
 *****************************************************************************/

using System;
using System.Threading;

internal class ThreadedClass
{
    // This flag is used to determine whether or not a Sleep call should 
    // include Timeout.Infinite. 
    private bool _waitIndefinitely = false;

    // This is the name of the thread as it runs.
    private string _threadName;

    internal ThreadedClass(string threadName)
    {
        ThreadName = threadName;
    }

    internal void ThreadMethod()
    {
        Console.WriteLine("\n\t{0}: Started!\n", ThreadName);

        // Check to see if the thread should wait forever.
        if (WaitIndefinitely)
        {
            Console.WriteLine ("\n\t{0}: Sleeping indefinitely. Time: {1}.\n",
                ThreadName, 
                DateTime.Now.ToString("T"));

            // Notice the try..catch block. This will catch the interrupt
            // exception if another thread interrupts this thread.
            try
            {
                // Sleep forever.
                Thread.Sleep (Timeout.Infinite);
            }
            catch (Exception e)
            {
                Console.WriteLine("\n\t{0}: Exception {1} caught. Details: {2}",
                    ThreadName,
                    e.ToString(),
                    e.Message);
            }
        }
        else
        {
            Console.WriteLine("\n\t{0}: Sleeping for 10 seconds. Time: {1}.\n",
                ThreadName,
                DateTime.Now.ToString("T"));

            // Sleep for 10 seconds.
            Thread.Sleep (10000);
        }

        Console.WriteLine("\n\t{0}: Just woke up. Time: {1}.\n", ThreadName, DateTime.Now.ToString("T"));
        Console.WriteLine("\n\t{0}: Thread ending.\n", ThreadName);
    }

    // This property provides access to the corresponding
    // underlying member variable.
    internal bool WaitIndefinitely
    {
        get { return _waitIndefinitely; }
        set { _waitIndefinitely = value; }
    }

    // This property provides access to the thread's name.
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
        // First, set the name of the current thread so that it's easier
        // to view in the debugger.
        Thread mainThread = Thread.CurrentThread;
        mainThread.Name = "Main Thread";

        // Create an instance of the ThreadedClass.
        ThreadedClass tc1 = new ThreadedClass("Thread1");

        // Set the ThreadedClass's boolean wait forever flag to false.
        tc1.WaitIndefinitely = false;

        // Create the thread giving it the delegate with the 
        // method that should be called when the thread starts.
        Console.WriteLine("Main - Creating first worker thread.");
        Thread t1 = new Thread(tc1.ThreadMethod);
        t1.Name = "First Worker Thread";

        // Start the thread
        t1.Start();
        Console.WriteLine ("Main - First worker thread started.");

        // Sleep for a few seconds.
        Console.WriteLine("Main - Sleeping for 5 seconds.");
        Thread.Sleep (5000);

        // Start another thread
        ThreadedClass tc2 = new ThreadedClass("Thread2");
        tc2.WaitIndefinitely = true;
        Console.WriteLine ("Main - Creating second worker thread.");
        Thread t2 = new Thread(tc2.ThreadMethod);
        t2.Name = "Second Worker Thread";
        t2.Start();
        Console.WriteLine ("Main - Second worker thread started.");

        // Sleep for a few seconds.
        Console.WriteLine("Main - Sleeping for 5 seconds.");
        Thread.Sleep(5000);

        // Now wake the second worker thread.
        Console.WriteLine("Main - Interrupting Thread2.");
        t2.Interrupt();

        // Sleep for a few more seconds to give the thread time to finish up.
        Console.WriteLine("Main - Sleeping for 10 seconds.");
        Thread.Sleep(10000);

        Console.Write ("Main - Press <ENTER> to end: ");
        Console.ReadLine();
    }
}

