using System;

internal class TimerEventArgs : EventArgs
{
    private DateTime _currentTime;
    internal TimerEventArgs()
    {
        _currentTime = DateTime.Now;
    }

    internal string CurrentTime
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

internal class CustomTimerClass
{
    internal delegate void TimerAlarm(object sender, TimerEventArgs e);
    internal event TimerAlarm OnTimerAlarm;

    // Objects can unsubscribe to the event at any time. In 
    // addition, other objects can stop this class' loop through
    // the stop flag.
    private bool _stopFlag = false;

    // This method will run until the stop flag is set to true.
    internal void Run()
    {
        while (!StopFlag)
        {
            // Sleep for a 10th of a second.
            System.Threading.Thread.Sleep (1000);

            // Raise the event.
            RaiseTimerAlarm ();
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
    private void RaiseTimerAlarm()
    {
        if (OnTimerAlarm != null)
        {
            Console.WriteLine();
            TimerEventArgs t = new TimerEventArgs();
            OnTimerAlarm (this, t);
        }
    }
}

internal class DisplayMessageClass
{
    static int _callCounter = 0;
    private CustomTimerClass _ctc;

    internal DisplayMessageClass(CustomTimerClass ctc)
    {
        _ctc = ctc;
        _ctc.OnTimerAlarm += new CustomTimerClass.TimerAlarm (DisplayMessage);
    }

    internal void DisplayMessage(object sender, TimerEventArgs e)
    {
        Console.WriteLine (e.CurrentTime + " - Called from the event delegate in the CustomTimerClass.");
        _callCounter++;

        if (_callCounter >= 10)
            _ctc.StopFlag = true;
    }
}

internal class LogMessageClass
{
    private CustomTimerClass _ctc;

    internal LogMessageClass(CustomTimerClass ctc)
    {
        _ctc = ctc;
        _ctc.OnTimerAlarm += new CustomTimerClass.TimerAlarm (LogMessage);
    }

    internal void LogMessage(object sender, TimerEventArgs e)
    {
        Console.WriteLine (e.CurrentTime + " - Logging message from the event delegate in the CustomTimerClass.");
    }
}

internal class TestClass
{
    static void Main(string[] args)
    {
        CustomTimerClass ctc = new CustomTimerClass();

        DisplayMessageClass dmc = new DisplayMessageClass (ctc);
        LogMessageClass lmc = new LogMessageClass (ctc);

        // Run the CustomTimerClass.
        ctc.Run();

        Console.Write ("\nPress <ENTER> to end: ");
        Console.Read();
    }
}

