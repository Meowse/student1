/******************************************************************************
 *  File: AutoResetEventDemo.cs
 * 
 *  This demonstration shows how to use the AutoResetEvent object. Main() will
 *  create the object and pass it to the ThreadedClass via its constructor.
 *  Once Main() has started the thread, it will call WaitOne() and wait for 
 *  the thread to end. When the thread ends, it will call Set(), which will
 *  raise the event and signal Main() that it is allowed to continue.
 * 
 *  As a side demonstration, ThreadedClass also takes a boolean when its
 *  constructor is called. When this parameter is set to true, the thread 
 *  will not call the Set() method. This demostrates how Main() could hang 
 *  forever and why it's important to call WaitOne() with some timeout 
 *  value, even if it's several minutes or more.
 * 
 *****************************************************************************/

using System;
using System.Threading;

internal class ThreadedClass
{
    // The AutoResetEvent object will be provided by the caller through the
    // constructor.
    private AutoResetEvent _completedEvent;
    private string _threadName;
    private bool _skipSet = false;

    internal ThreadedClass(string threadName, AutoResetEvent are, bool skipSet)
    {
        ThreadName = threadName;
        _completedEvent = are;
        _skipSet = skipSet;
    }

    internal void ThreadMethod()
    {
        Console.WriteLine("\n\t{0}: Started!", ThreadName);

        try
        {
            for (int i=0; i < 10; i++)
            {
                Console.WriteLine("\n\t{0}: Reading record {1}.", ThreadName, i+1);
                Thread.Sleep(1000);
                Console.WriteLine("\n\t{0}: Processing record.", ThreadName);
            }
        }
        catch (ThreadAbortException tae)
        {
            Console.WriteLine("\t{0}: Exception {1} caught. Details: {2}\n",
                ThreadName,
                tae.ToString(),
                tae.Message);

            Thread.Sleep(5000);
        }
        catch (Exception e)
        {
            Console.WriteLine("\t{0}: Exception {1} caught. Details: {2}\n",
                ThreadName,
                e.ToString(),
                e.Message);
        }
        finally
        {
            Console.WriteLine("\n\t{0}: Performing iteration cleanup.", ThreadName);

            if (!_skipSet)
            {
                Console.WriteLine("\n\t{0}: Calling Set() to signal waiting threads.", ThreadName);
                _completedEvent.Set();
            }
        }
    }

    private string ThreadName
    {
        get { return _threadName; }
        set { _threadName = value; }
    }
}

class TestClass
{
    public static AutoResetEvent threadCompleted = new AutoResetEvent(false);

    static void Main()
    {
        try
        {
            Thread.CurrentThread.Name = "Main Thread";

            // Create an instance of the ThreadedClass.
            ThreadedClass tc1 = new ThreadedClass("Thread1", threadCompleted, false);

            // Create the thread giving it the delegate with the 
            // method that should be called when the thread starts.
            Console.WriteLine ("\nMain - Creating thread T1.");
            Thread t1 = new Thread(tc1.ThreadMethod);
            t1.Name = "Worker Thread";

            // Start the thread and then pause for a moment.
            t1.Start();
            Console.WriteLine("\nMain - T1 thread started.");
            Thread.Sleep(1000);

            // Now wait until the thread completes.
            Console.WriteLine("\nMain - Waiting for signal from T1.");
            if (TestClass.threadCompleted.WaitOne())
            {
                Console.WriteLine("\nMain - Got signal from T1.");
            }
            else
            {
                Console.WriteLine("\nMAIN - WaitOne() for T1 timed out!!!!");
            }

            Console.Write("\nPress <ENTER> to continue: ");
            Console.ReadLine();
            Console.Clear();

            // Now, we'll test this again, but this time we'll avoid calling Set()
            // in the thread. See what happens to Main().

            tc1 = new ThreadedClass("Thread2", threadCompleted, true);

            // Create the thread giving it the delegate with the 
            // method that should be called when the thread starts.
            Console.WriteLine("\nMain - Creating thread T2.");
            Thread t2 = new Thread(tc1.ThreadMethod);
            t2.Name = "Worker Thread";

            // Start the thread and then pause for a moment.
            t2.Start();
            Console.WriteLine("\nMain - T2 thread started.");
            Thread.Sleep(1000);

            // Now wait until the thread completes.
            Console.WriteLine("\nMain - Waiting for signal from T2.");
            if (TestClass.threadCompleted.WaitOne(15000, false))
            {
                Console.WriteLine("\nMain - Got signal from T2.");
            }
            else
            {
                Console.WriteLine("\nMAIN - WaitOne() for T2 timed out!!!!");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine ("\nMAIN - EXCEPTION: {0}", e.Message);
        }
        finally
        {
            // Pause the program. Usually, this message will appear
            // before the message from the threaded method appears.
            Console.Write ("\n *** Main - Press <ENTER> to end: ");
            Console.ReadLine();
        }
    }
}

