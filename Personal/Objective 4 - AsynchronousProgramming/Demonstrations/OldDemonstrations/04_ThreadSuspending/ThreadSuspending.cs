/*****************************************************************************
 * 
 *  This program demonstrates how to suspend a thread and then resume it 
 *  from a suspended state.
 * 
 *  Note: Suspend() and Resume() are deprecated as of .NET 2.0. Warnings
 *  will occur when this code is compiled.
 * 
 *****************************************************************************/

using System;
using System.Threading;

internal class ThreadedClass
{
    private bool _internalSuspendCall = false;
    private string _threadName;

    internal ThreadedClass(string threadName)
    {
        ThreadName = threadName;
    }

    internal void ThreadMethod()
    {
        Console.WriteLine("\n\t{0}: Started!\n", ThreadName);

        // See if the currently-running thread should call Suspend on itself.
        if (!InternalSuspendCall)
        {
            Console.WriteLine("\n\t{0}: Allowing Main to suspend me. Time: {1}.\n",
                ThreadName, 
                DateTime.Now.ToString("T"));

            try
            {
                // Give up the time-slice to allow Main to suspend this thread.
                Thread.Sleep (5000);
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
            Console.WriteLine("\n\t{0}: Suspending myself. Time: {1}.\n",
                ThreadName,
                DateTime.Now.ToString("T"));

            // Notice the use of the try..catch block.
            try
            {
                Thread currentThread = Thread.CurrentThread;
                currentThread.Suspend();
            }    
            catch (Exception e)
            {
                Console.WriteLine("\n\t{0}: Exception {1} caught. Details: {2}",
                    ThreadName,
                    e.ToString(),
                    e.Message);
            }
        }

        Console.WriteLine("\n\t{0}: Just woke up. Time: {1}.\n", ThreadName, DateTime.Now.ToString("T"));
        Console.WriteLine("\n\t{0}: Thread ending.\n", ThreadName);
    }

    // Property that provides access to the corresponding
    // underlying member variable.
    internal bool InternalSuspendCall
    {
        get { return _internalSuspendCall; }
        set { _internalSuspendCall = value; }
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
        // Get the current thread and set its name.
        Thread.CurrentThread.Name = "Main Thread";

        // Create an instance of the ThreadedClass.
        ThreadedClass tc1 = new ThreadedClass("Thread1");

        // Set the ThreadedClass's boolean internal suspend call to true.
        tc1.InternalSuspendCall = true;

        // Create the thread giving it the delegate with the 
        // method that should be called when the thread starts.
        Console.WriteLine ("Main - Creating thread t1.");
        Thread t1 = new Thread(tc1.ThreadMethod);
        t1.Name = "T1 Worker Thread";

        // Start the thread
        t1.Start();
        Console.WriteLine ("Main - t1 thread started.");

        // Sleep for a few seconds.
        Console.WriteLine("Main - Sleeping for 5 seconds.");
        Thread.Sleep (5000);

        // Start another thread
        ThreadedClass tc2 = new ThreadedClass("Thread2");
        tc2.InternalSuspendCall = false;
        Thread t2 = new Thread(tc2.ThreadMethod);
        t2.Name = "T2 Worker Thread";

		t2.Start();
        Console.WriteLine ("Main - t2 thread started.");

        // Resume the first thread.
        Console.WriteLine ("Main - Resuming t1 from Suspend() call.");
        t1.Resume();

        // Pause the second thread.
        Console.WriteLine ("Main - Calling Suspend() on t2.");      // *** For some reason, I occasionally see this program hang here when not debugging.
        t2.Suspend();   // This may throw an exception if t2 is not running.
        Console.WriteLine ("Main - Thread t2 is now suspended.");

        // Wait a moment.
        Console.WriteLine("Main - Sleeping for 5 seconds.");
        Thread.Sleep(5000);

        // Resume the second thread.
        Console.WriteLine ("Main - Resuming t2 from Suspend() call.");
        t2.Resume();

        // Wait a moment.
        Console.WriteLine("Main - Sleeping for 5 seconds.");
        Thread.Sleep(5000);

        // Pause the program. Usually, this message will appear
        // before the message from the threaded method appears.
        Console.Write ("Main - Press <ENTER> to end: ");
        Console.ReadLine();
    }
}

