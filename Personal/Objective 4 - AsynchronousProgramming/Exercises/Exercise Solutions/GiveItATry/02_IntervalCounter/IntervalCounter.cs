using System;
using System.Threading;

// This class will run the loop and at each interval, display a message
// with the current value of the counter.
internal class IntervalCounter
{
    private int _interval;  // How high to count to.
    private int _divisor;   // The number to divide into interval. If the answer is 0,
                            //  a message will be displayed to the console.

    // Constructor that takes the interval and divisor values.
    internal IntervalCounter (int interval, int divisor)
    {
        Interval = interval;
        Divisor = divisor;
    }

    // This method will be called on a second thread.
    internal void DisplayIntervals ()
    {
        // Get a reference to the current thread.
        Thread current = Thread.CurrentThread;

        // Display the thread's name.
        Console.WriteLine ("\n\nDisplay interval information on thread: {0}.\n\n", current.Name);

        // Run the loop.
        for (int i=1; i <= Interval; i++)
        {
            // Check if the counter is evenly divisible by the divisor.
            if (i % Divisor == 0)
            {
                // Display a status message.
                Console.WriteLine ("\tCount has reached {0}.", i);
            }
        }
    }

    private int Interval
    {
        get { return _interval; }
        set { _interval = value; }
    }

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

            // Create the thread object. You can skip creating the delegate
            // object and simply provide the target method to the Thread's
            // constructor. The delegate object will be created by the runtime.
            Thread workerThread = new Thread(ic.DisplayIntervals);

            // Set the name of the thread.
            workerThread.Name = "Interval Counter Worker Thread";

            // Start the thread.
            workerThread.Start();

            // Join the thread and wait until it is done.
            workerThread.Join();
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
