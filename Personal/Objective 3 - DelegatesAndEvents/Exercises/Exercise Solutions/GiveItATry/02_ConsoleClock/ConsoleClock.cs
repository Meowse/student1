/*****************************************************************************
 * 
 *  This code adds another feature. Instead of displaying the current time for
 *  each second on its own separate line, this program will set the cursor
 *  position to the same line for each write. The result is that the time will
 *  be displayed on the same line for each second. Search for ADDED FEATURE:
 *  in the comments below to see how this works.
 * 
 ****************************************************************************/

using System;

// Create a custom Event Arguments class.
internal class TimerEventArgs : EventArgs
{
    private int _currentIteration = 0;

    // Constructor for our custom event args class.
    internal TimerEventArgs(int i)
    {
        _currentIteration = i;
    }

    // A read-only property that retrieves the 
    // counter value.
    internal int CurrentIteration
    {
        get { return _currentIteration; }
    }
}

// Declare the delegate. Notice that I chose not to declare
// it inside of another type such as a class.
internal delegate void UpdateClockHandler(object sender, TimerEventArgs tea);

// This class will handle the actual timer. It will display
// the counter that it's using, but it's not going to display
// any other output. That will be up to the subscriber of the
// event.
internal class ConsoleClock
{
    // Declare our event.
    internal event UpdateClockHandler OnUpdateClock;

    // This counter will be used to (a) kill the infinite loop and
    // (b) display the current iteration the loop is on.
    private int _counter = 1;

    // This method is called by the subscriber to start the time.
    internal void RunTheClock()
    {
        // Loop until counter hits 100.
        while (_counter <= 100)
        {
            // Sleep for a second.
            System.Threading.Thread.Sleep (1000);

            // See if there are any subscribers.
            if (null != OnUpdateClock)
            {
                // Create a new TimerEventArgs object providing the 
                // value of the counter.
                TimerEventArgs tea = new TimerEventArgs(_counter);

                // Signal the subscribers.
                OnUpdateClock (this, tea);
            }

            // Increment the counter.
            _counter++;
        }
    }
}

internal class Tester
{
    private static int _top = 0;
    private static int _left = 0;

    internal void TesterUpdateClockHandler(object sender, TimerEventArgs tea)
    {
        // ADDED FEATURE: Set the cursor position in the console window.
        Console.SetCursorPosition(_top, _left);

        // Display the value stored in the TimerEventArgs.
        Console.Write ("{0}. ", tea.CurrentIteration.ToString("000"));

        // Display the current time.
        DateTime dt = DateTime.Now;
        Console.WriteLine ("{0}:{1}:{2}", 
            dt.Hour.ToString("00"),
            dt.Minute.ToString("00"),
            dt.Second.ToString("00"));
    }

    static void Main()
    {
        // ADDED FEATURE: Clear the console window.
        Console.Clear();

        // Save the top and left coordinates.
        _top = Console.CursorTop;
        _left = Console.CursorLeft;

        // Create a Tester object.
        Tester t = new Tester();

        // Create a ConsoleClock object.
        ConsoleClock cc = new ConsoleClock();

        // Subscribe to the event in the ConsoleClock.
        cc.OnUpdateClock += new UpdateClockHandler (t.TesterUpdateClockHandler);

        // Start the timer.
        cc.RunTheClock();

        // Display this when the program is shutting down.
        Console.Write ("\nPress <ENTER> to end: ");
        Console.ReadLine();
    }
}
