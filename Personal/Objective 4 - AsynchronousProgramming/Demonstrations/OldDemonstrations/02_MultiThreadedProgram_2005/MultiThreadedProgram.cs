/*****************************************************************************
 * 
 *  This sample demonstrates creating a Thread through the use of anonymous
 *  methods.
 * 
 *****************************************************************************/

using System;
using System.Threading;

class TestClass
{
    static void Main()
    {
        Console.WriteLine ("Main - Creating worker thread.");

        // In this case we were able to eliminate the the 
        // entire ThreadedClass class. Instead, by using an
        // anonymouse method, we included the method code
        // right here along with creating the instance of
        // the thread object.
        Thread t1 = new Thread
        (
            delegate()
            {
                // Sleep for 1.5 seconds.
                Thread.Sleep(1500);

                // Display a message to the console.
                Console.WriteLine("\n\n{0} - In the thread method.", Thread.CurrentThread.Name);
            }
        );

        // Give the thread object a friendly name.
        t1.Name = "ThreadMethodOne";

        // Start the thread.
        t1.Start();
        Console.WriteLine ("Main - Thread #1 started.");

        Thread t2 = new Thread
        (
            delegate(object o)
            {
                // Sleep for 1.5 seconds
                Thread.Sleep(1500);

                // Display a message to the console.
                Console.WriteLine("\n\n{0} - {1}", Thread.CurrentThread.Name, o.ToString());
            }
        );

        // Give the thread object a friendly name.
        t2.Name = "ThreadMethodTwo";

        // Start the thread.
        t2.Start("[ Data that was passed via ParameterizedThreadStart! ]");
        Console.WriteLine("Main - Thread #2 started.");

        // Pause the program. Usually, this message will appear
        // before the message from the threaded method appears.
        Console.Write ("Main - Press <ENTER> to end: ");
        Console.ReadLine();
    }
}

