/*******************************************************************
 * 
 *  In this program, we create a new class called TimerEventArgs which
 *  allows us to pass information when the event fires from the sender
 *  to the subscribers.
 * 
 *******************************************************************/

using System;

// This class inherits from the base class EventArgs. This class will
// get the current system time and store it. If the event handler wishes
// to know the exact time the event fired, it can call the FireTime
// public property which will format the current time into readable text.
internal class TimerEventArgs : EventArgs
{
    private DateTime _currentTime;
    internal TimerEventArgs()
    {
        _currentTime = DateTime.Now;
    }

    internal string FireTime
    {
        get
        {
            string time;
            time = _currentTime.Hour.ToString("00") + ":" +
                _currentTime.Minute.ToString("00") + ":" +
                _currentTime.Second.ToString("00") + "." +
                _currentTime.Millisecond.ToString("000");
            return time;
        }
    }
}

// This class contains the delegate and the event. This is the
// publisher class. Other objects will subscribe to this class'
// event.
internal class TimerClass
{
    // Notice that this delegate now uses TimerEventArgs instead
    // of EventArgs. An object of TimerEventArgs will be passed to
    // the event handler when the event fires.
    internal delegate void AdvanceTimerHandler(object sender, TimerEventArgs e);

    // This is the event that will be raised. The event that
    // can be subscribed to is OnAdvanceTimer.
    internal event AdvanceTimerHandler OnAdvanceTimer;

    // Objects can unsubscribe to the event at any time. In 
    // addition, other objects can stop this class' loop through
    // the stop flag.
    private bool _stopFlag = false;

    // This method will run until the stop flag is set to true.
    internal void Run ()
    {
        while (!StopFlag)
        {
            // Sleep for a 100 milliseconds.
            System.Threading.Thread.Sleep (100);

            // Call the method that will raise the event.
            RaiseTimerEvent ();
        }
    }

    // Public property to stop the infinite loop above.
    internal bool StopFlag
    {
        get { return _stopFlag; }
        set { _stopFlag = value; }
    }

    // Internal helper method that checks to see if any
    // objects are subscribed to the event. If any are,
    // their event handler method will be called through
    // the multicast delegate.
    private void RaiseTimerEvent()
    {
        if (OnAdvanceTimer != null)
        {
            // Note that before the event fires, an object of
            // type TimerEventArgs is created. Then, when the
            // event is fired, the TimerEventArgs object is 
            // passed along with the sender.
            TimerEventArgs t = new TimerEventArgs();
            OnAdvanceTimer (this, t);
        }
    }
}

internal class EventsTestClass
{
    // Some local member variables.
    private int _counter = 0;
    private string _displayString = "This string will be displayed one letter at a time.";
    private TimerClass _tc;

    // Constructor for the class.
    internal EventsTestClass(TimerClass timer)
    {
        // Obtain a reference to the Timer class.
        _tc = timer;

        // Subscribe to the event in the Timer class and
        // provide it the handler that will be called when
        // the TimerClass raises the event. Note that if
        // you change the operator from '+=' to just '=',
        // the compiler will generate an error. This shows
        // that the event keyword enforces the use of the
        // appropriate operator.
        _tc.OnAdvanceTimer += new TimerClass.AdvanceTimerHandler (DisplayLetterHandler);
    }

    // Now the event hander will be called with a TimerEventArgs
    // object. The handler can choose to ignore the data in this 
    // object, or it can use the data. In this case, we'll use 
    // the data as part of the output.
    internal void DisplayLetterHandler(object sender, TimerEventArgs e)
    {
        if (_counter < _displayString.Length)
        {
            Console.Write ("\nEventsTestClass: " + e.FireTime + " - " + _displayString[_counter++ % _displayString.Length]);
        }
        else
        {
            _tc.OnAdvanceTimer -= new TimerClass.AdvanceTimerHandler (DisplayLetterHandler);
            _tc.StopFlag = true;
        }
    }

    static void Main()
    {
        // Create a TimerClass object.
        TimerClass tc = new TimerClass();

        // Create an EventsTestClass object providing a reference to
        // the TimerClass object.
        EventsTestClass etc = new EventsTestClass(tc);

        // Create AnotherEventTestClass object and provide it the
        // same reference to the TimerClass object.
        AnotherEventTestClass aetc = new AnotherEventTestClass(tc);

        // Run the TimerClass.
        tc.Run();

        Console.Write ("\n\nPress <ENTER> to end: ");
        Console.Read();
    }
}

// This class demostrates how multiple objects can subscribe to the
// same event.
internal class AnotherEventTestClass
{
    private TimerClass _tc;

    // Constructor for the class.
    internal AnotherEventTestClass(TimerClass timer)
    {
        // Obtain a reference to the Timer class.
        _tc = timer;

        // Subscribe to the event in the Timer class providing
        // the handler method that will be called when the Timer
        // class raises the event.
        _tc.OnAdvanceTimer += new TimerClass.AdvanceTimerHandler (DisplayMessageHandler);
    }

    // In this case, we chose to ignore the data found in the
    // TimerEventArgs object. If we need the data, we have access
    // to it later.
    internal void DisplayMessageHandler(object sender, TimerEventArgs e)
    {
        Console.WriteLine ("\nAnotherEventTestClass: Called from the event delegate in the TimerClass.");
    }
}
