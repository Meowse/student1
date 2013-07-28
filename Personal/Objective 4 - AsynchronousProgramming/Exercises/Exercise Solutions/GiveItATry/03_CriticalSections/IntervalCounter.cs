/******************************************************************************
 * 
 *  Instructions:
 * 
 *  There are four steps to this exercise. After each step, recompile and rerun
 *  the code and record (a) the number of iterations the interval loop had to
 *  make before it ended and (b) the amount of time that elapsed between the 
 *  time the loop started and the time it ended. Here are the four steps:
 * 
 *      1. In Main(), look at the TODO comments and implement the code that
 *         each describes. Run the application and record the information.
 *      2. In the UpdateCounter method, locate the TODO comment with Step 2
 *         and implement the code that it describes. Rerun the application and
 *         record the information.
 *      3. In DisplayInterval, locate the TODO comment with Step 3 and 
 *         implement the code that it describes. In UpdateCounter, locate the
 *         TODO comment with Step 3 and implement the code that it describes.
 *         Rerun the application and record the information.
 *      4. In UpdateCounter(), locate the TODO comment with Step 4 and implement
 *         the code that it describes. Rerun the application and record the
 *         information.
 * 
 *  Based on the four different scenarios, which one has the best performance?
 *  Knowing the performance numbers of each scenario, what is it about the 
 *  design of the best performing scenario that makes it so good?
 * 
 ******************************************************************************/

using System;
using System.Threading;

// This class will run the loop and at each interval, display a message
// with the current value of the counter.
internal class IntervalCounter
{
    private static int _counter;    // Will increment for each loop.
    private static int _interval;   // How high to count to.
    private static int _divisor;    // The number to divide into interval. If the answer is 0,
                                    //  a message will be displayed to the console.

    // Constructor.
    internal IntervalCounter(int i, int d)
    {
        Interval = i;
        Divisor = d;
        _counter = 1;
    }

    // Static property that represents the interval member.
    private static int Interval
    {
        get { return _interval; }
        set { _interval = value; }
    }

    // Static property that represents the divisor member.
    private int Divisor
    {
        get { return _divisor; }
        set
        {
            if (value == 0)
            {
                Console.WriteLine("\nWARNING: Divisor is 0. Setting to 100.");
                _divisor = 100;
            }
            else
            {
                _divisor = value;
            }
        }
    }

    // This method will be called on a second thread.
    internal void DisplayIntervals()
    {
        int totalLoops = 0;
        int loopCounter = 0;

        // TODO: Step 3a - Add a lock around all of the following code. Use the C# lock keyword.
        lock (this)
        {
            // Get a reference to the current thread.
            Thread current = Thread.CurrentThread;

            // Display the thread's name.
            Console.WriteLine ("\n\nDisplay interval information on thread: {0}.", current.Name);
            Console.WriteLine ("Start interval loop at: {0}.\n\n", GetCurrentTime());

            // Run the loop.
            while (_counter <= Interval)
            {
                loopCounter++;
                // Check if the counter is evenly divisible by the divisor.
                if (_counter % Divisor == 0)
                {
                    // Display a status message.
                    Console.WriteLine ("    {0}: Count has reached {1}. Loop ran {2} times.",
                        current.Name, _counter.ToString("N0"), loopCounter.ToString("N0"));

                    totalLoops += loopCounter;
                    loopCounter = 0;
                }

                // Update the counter.
                UpdateCounter();
            }

            Console.WriteLine ("\n\nTotal iterations needed to complete job: {0}.", totalLoops.ToString("N0"));
            Console.WriteLine ("{0}: Ending interval loop at: {1}.", current.Name, GetCurrentTime());
        }
    }

    private void UpdateCounter ()
    {
        // TODO: Step 2 - Add a lock around the following line of code. Use the C# lock keyword.
        // TODO: Step 3b - Comment out the lock call, but leave the ++ operation in.
        // TODO: Step 4 - Uncomment the lock call.
        lock (this)
        {
            _counter++;
        }
    }

    private string GetCurrentTime ()
    {
        DateTime dt = DateTime.Now;
        string time = dt.Hour + ":" + dt.Minute.ToString("00") + ":" + dt.Second.ToString("00") +
            "." + dt.Millisecond.ToString("000");

        return time;
    }
}

class TesterClass
{
    static void Main()
    {
        int interval;
        int divisor;

        try
        {
            // Prompt for the interval - the number to count up to.
            Console.Write ("Enter the interval (e.g. 100000000):  ");
            interval = int.Parse(Console.ReadLine());

            // Prompt for the divisor - the number that will be divided into 
            // the counter. If the result is 0, a status message will be displayed.
            Console.Write ("Enter the divisor  (e.g. 10000000):   ");
            divisor = int.Parse(Console.ReadLine());

            // Create a new IntervalCounter object passing in the data from
            // the user.
            IntervalCounter ic = new IntervalCounter(interval, divisor);

            // Create the thread delegate giving it the target method to call.
            ThreadStart workerDelegate1 = new ThreadStart(ic.DisplayIntervals);

            // TODO: Step 1a - Create a second thread delegate.
            ThreadStart workerDelegate2 = new ThreadStart(ic.DisplayIntervals);

            // Create the thread object.
            Thread workerThread1 = new Thread(workerDelegate1);

            // TODO: Step 1b - Create a second thread object giving it the second delegate.
            Thread workerThread2 = new Thread(workerDelegate2);

            // Set the name of the threads.
            workerThread1.Name = "Worker Thread #1";

            // TODO: Step 1c - Set the name of the second thread.
            workerThread2.Name = "Worker Thread #2";

            // Start the thread.
            workerThread1.Start();

            // TODO: Step 1d - Start the second thread.
            workerThread2.Start();

            // Join the thread and wait until it is done.
            workerThread1.Join();

            // TODO: Step 1e - Join the second thread and wait until it is done.
            workerThread2.Join();
        }
        catch (Exception e)
        {
            Console.WriteLine ("\n\nERROR: Exception [ " + e.Message + " ] occurred.");
        }

        // Pause to view output.
        Console.Write("\n\nPress <ENTER> to end: ");
        Console.ReadLine();
    }
}
