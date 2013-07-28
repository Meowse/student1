/*****************************************************************************
 * 
 *  This sample demonstrations how to start a thread with both a ThreadStart
 *  delegate and a ParameterizedThreadStart delegate. In both cases, the
 *  delegate is explicitly created and then supplied to the Thread object
 *  before the thread is started.
 * 
 *****************************************************************************/

using System;
using System.Threading;

// A class containing the methods that will be called
// when a thread starts.
internal class ThreadedClass
{
    internal static void ThreadMethodOne()
    {
        // Sleep for 1.5 seconds.
        Thread.Sleep (1500);

        // Display a message to the console.
        Console.WriteLine("\n\nThreadMethodOne - In the thread method.");
    }

    internal static void ThreadMethodTwo(object o)
    {
        // Sleep for 1.5 seconds
        Thread.Sleep(1500);

        // Display a message to the console.
        Console.WriteLine("\n\nThreadMethodTwo - {0}", o.ToString());
    }
}

class TestClass
{
    static void Main()
    {
        // Create the delegate pointing it to the method that 
        // will be called when the thread starts. In this case 
        // we'll use the traditional ThreadStart delegate
        ThreadStart workerThread = new ThreadStart(ThreadedClass.ThreadMethodOne);
        Console.WriteLine ("Main - Creating thread #1.");

        // Create the thread giving it the delegate with the 
        // method that should be called when the thread starts.
        Thread t1 = new Thread(workerThread);
        t1.Name = "Thread #1";

        // Start the thread.
        t1.Start();
        Console.WriteLine ("Main - Thread #1 started.");

        // Now create another thread using the ParameterizedThreadStart
        // delegate and pass some data to the method when the thread
        // starts up.
        Console.WriteLine("Main - Creating thread #2.");
        string data = "[ Data that was passed via ParameterizedThreadStart! ]";
        Thread t2 = new Thread(ThreadedClass.ThreadMethodTwo);
        t2.Name = "Thread #2";
        t2.Start(data);
        Console.WriteLine("Main - Thread #2 started.");

        // Pause the program. Usually, this message will appear
        // before the message from the threaded method appears.
        Console.Write ("\nMain - Press <ENTER> to end: ");
        Console.ReadLine();
    }
}

