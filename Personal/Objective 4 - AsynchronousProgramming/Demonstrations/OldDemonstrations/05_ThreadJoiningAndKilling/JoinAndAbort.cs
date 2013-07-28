/******************************************************************************
 *  File: JoinAndAbort.cs
 * 
 *  To run this demo:
 * 
 *      First, compile and run it like it is and see how the thread will keep
 *      looping because of the GOTO instruction. When MAIN() ends, we can 
 *      press <ENTER>, but the program will continue to run until the thread
 *      ends.
 * 
 *      Second, uncomment the code in Main() on lines 147-155. This will 
 *      demonstrate how an abort followed by a join works. Main() calls Abort() 
 *      which interrupts the thread's execution, the goto branch is ignored 
 *      and the thread ends.
 * 
 *      Finally, uncomment the Thread.Sleep (20000); line in the ThreadMethod.
 *      This will pause the thread after the Abort exception is caught. Notice
 *      how this changes the behavior of Main(). Even though the thread was
 *      aborted, the Sleep() call pauses the thread, which holds up Main()
 *      from exiting immediately.
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

    internal bool InternalSuspendCall
    {
        get { return _internalSuspendCall; }
        set { _internalSuspendCall = value; }
    }

    internal void ThreadMethod()
    {
        int counter = 0;

        Console.WriteLine("\n{0}: Started!\n", ThreadName);

        // Let's write some bad code! Start off with a label.

PerformAction:

        try
        {
            // Use this counter to simulate a long-running task.
            counter++;

            // Simulate some work.
            for (long i=0; i < 100; i++)
            {
                if (i%10 == 0)
                {
                    Console.WriteLine("\n{0}: 'counter' = {1} and 'i' = {2}.\n", ThreadName, counter, i);
                }

                Thread.Sleep (100);
            }

            throw new Exception(ThreadName + ": Fake exception after working.");
        }
        catch (ThreadAbortException tae)
        {
            Console.WriteLine ("{0}: Exception {1} caught. Details: {2}\n",
                ThreadName,
                tae.ToString(),
                tae.Message);

            // Uncomment the following line once the Join() code has been added 
            // to the program.
            //Thread.Sleep(20000);
        }
        catch (Exception e)
        {
            Console.WriteLine ("{0}: Exception {1} caught. Details: {2}\n",
                ThreadName,
                e.ToString(),
                e.Message);

            if (counter < 5)
            {
                // Because of this goto, the finally code will be executed but
                // the program pointer will go to the PerformAction label. Unless
                // the exception is a ThreadAbortException - in which case the
                // goto instruction below will be ignored.
                goto PerformAction;
            }
        }
        finally
        {
            Console.WriteLine("\n{0}: Performing iteration cleanup.\n", ThreadName);
        }

        // Pause for a few moments.
        Thread.Sleep (1000);

        // Finish up.
        Console.WriteLine("\n{0}: Just woke up. Time: {1}.\n", ThreadName, DateTime.Now.ToString("T"));
        Console.WriteLine("\n{0}: Thread ending.\n", ThreadName);
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
        try
        {
            Thread.CurrentThread.Name = "Main Thread";

            // Create an instance of the ThreadedClass.
            ThreadedClass tc1 = new ThreadedClass("Thread1");

            // Set the ThreadedClass's boolean internal suspend call to true.
            tc1.InternalSuspendCall = true;

            // Create the thread giving it the delegate with the 
            // method that should be called when the thread starts.
            Console.WriteLine ("Main - Creating thread t1.");
            Thread t1 = new Thread(tc1.ThreadMethod);
            t1.Name = "Worker Thread";

            // Start the thread
            t1.Start();
            Console.WriteLine ("Main - T1 thread started.");

            // Pause for a few moments.
            Console.WriteLine ("Main - Sleeping for 5 seconds.");
            Thread.Sleep(5000);

            // Abort the thread.
            //Console.WriteLine("Main - Calling abort on T1.");
            //t1.Abort();

            //// Join the thread.
            //Console.WriteLine("Main - Joining T1.");
            //if (!t1.Join(5000))
            //{
            //    Console.WriteLine("\nMain - ERROR: thread did not terminate within allotted time!\n");
            //}
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

