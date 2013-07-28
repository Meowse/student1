/**********************************************************************
 * 
 * Part 10: DelegatesAndEvents
 * 
 * Topics:  1.  Implement a custom EventArgs class.
 **********************************************************************/             

using System;
using System.Collections.Generic;
using System.Text;

namespace EventsDemo
{
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

        internal string AlarmTime
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

    internal class TimerClass
    {
        // This is the delegate that encapsulates the target methods 
        // in other objects. When an object wishes to subscribe to the 
        // event in this class, this delegate will be used to call the 
        // other objects' event handler methods when the event is raised.
        internal delegate
            void TimerExpiredHandler(object sender, TimerEventArgs e);

        // This is the event (aka the multicast delegate) that will be 
        // raised. The event that can be subscribed to is OnAdvanceTimer.
        internal event TimerExpiredHandler OnTimerExpired;

        // This method will run until the stop flag is set to true.
        internal void Run(int numberOfSeconds)
        {
            int elapsedSeconds = 0;

            while (elapsedSeconds < numberOfSeconds)
            {
                // Sleep for a 10th of a second.
                System.Threading.Thread.Sleep(1000);

                // Increment the number of elapsed seconds;
                elapsedSeconds++;
            }

            // Now the the timer has expired, it's time to 
            // raise the alarm.
            RaiseAlarmEvent();
        }

        // Internal helper method that checks to see if any
        // objects are subscribed to the event. If any are,
        // their event handler method will be called through
        // the multicast delegate.
        private void RaiseAlarmEvent()
        {
            // Make sure to check that there is at least one subscriber!
            if (OnTimerExpired != null)
            {
                // Note that before the event fires, an object of
                // type TimerEventArgs is created. Then, when the
                // event is fired, the TimerEventArgs object is 
                // passed along with the sender.
                TimerEventArgs t = new TimerEventArgs();
                OnTimerExpired(this, t);
            }
        }
    }

    class EventTestClass
    {
        private void DisplayAlarmMessage(object sender, TimerEventArgs e)
        {
            // Save the current color of the characters.
            ConsoleColor originalForeGroundColor = Console.ForegroundColor;

            // Change the color of the characters to Red.
            Console.ForegroundColor = ConsoleColor.Red;

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("\n{0}: WAKE UP!!!!!!!!!!!", 
                    e.AlarmTime);
                Console.Beep(5000, 500);

                System.Threading.Thread.Sleep(1000);
            }

            // Reset the color of the characters to their original color.
            Console.ForegroundColor = originalForeGroundColor;
        }

        static void Main()
        {
            // Clear the console window.
            Console.Clear();

            // Get the number of seconds to wait.
            Console.Write("Number of seconds to put on the clock: ");
            string seconds = Console.ReadLine();
            int numberOfSeconds = 0;

            // Attempt to convert the string input into integer.
            if (int.TryParse(seconds, out numberOfSeconds))
            {
                // Create an instance of our first test class.
                EventTestClass firstTestClass = new EventTestClass();

                // Create an instance of our second test class.
                AnotherEventTestClass secondTestClass = 
                    new AnotherEventTestClass();

                // Create an instance of the timer class.
                TimerClass alarmClock = new TimerClass();

                // Subscribe the first test class to the event in 
                // the timer class.
                alarmClock.OnTimerExpired += 
                    new TimerClass.TimerExpiredHandler
                    (firstTestClass.DisplayAlarmMessage);

                // Subscribe the second test class to the event in 
                // the timer class.
                alarmClock.OnTimerExpired += 
                    new TimerClass.TimerExpiredHandler
                    (secondTestClass.ShowMessage);

                // Turn on the alarm clock.
                alarmClock.Run(numberOfSeconds);
            }

            Console.Write("\n\nPress <ENTER> to end: ");
            Console.ReadLine();
        }
    }

    class AnotherEventTestClass
    {
        internal void ShowMessage(object sender, TimerEventArgs e)
        {
            // Save the current color of the characters.
            ConsoleColor originalForeGroundColor = Console.ForegroundColor;

            // Change the color of the characters to Magenta.
            Console.ForegroundColor = ConsoleColor.Magenta;

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("\n{0}: This is your wakeup call.", 
                    e.AlarmTime);
                Console.Beep(3500, 400);

                System.Threading.Thread.Sleep(750);
            }

            // Reset the color of the characters to their original color.
            Console.ForegroundColor = originalForeGroundColor;
        }
    }
}
