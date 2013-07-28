/***************************************************************************
 * 
 * Part 9: Threading
 * 
 * Topic:  Using the ParameterizedThreadStart delegate to pass in a single
 *          object type argument.
 ***************************************************************************/

/****************************************************************************
 * 
 * Be sure to provide an integer value on the command line. To do this:
 * 
 *  1)  Right-click on the project in Solution Explorer and click
 *      Properties.
 * 
 *  2)  In the Properties window, click the Debug tab.
 * 
 *  3)  In the "Command line arguments" field, enter a whole number.
 *
 ***************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ThreadsDemo
{
    internal class ComplicatedCalculator
    {
        // Member variable that represent the number of milliseconds
        // to pause the thread.
        private int _millisecondsToPause;

        // Member variables that are used for the calculation.
        private double _results;

        public ComplicatedCalculator(int millisecondsToPause)
        {
            MillisecondsToPause = millisecondsToPause;
        }

        // Provide yet another method that takes a single object
        // argument. This will parse the object and get the input values
        // from it.
        internal void CalculateValue(object input)
        {
            // Attempt to convert the input object to an array of
            // doubles.
            double[] inputValues = input as double[];

            // If the conversion worked and there are at least two elements
            // in the double array, run the calculation.
            if (null != inputValues && inputValues.Length >= 2)
            {
                Results = CalculateValue(inputValues[0], inputValues[1]);
            }
        }

        // This method represents a task that could potentially run for
        // a long period of time.
        internal double CalculateValue
            (double firstNumber, double secondNumber)
        {
            double answer = 0;

            // Save the foreground color of the console window.
            ConsoleColor originalcolor = Console.ForegroundColor;

            // Change the foreground color in the console.
            Console.ForegroundColor = ConsoleColor.Red;

            // Display a message that we're starting the task.
            Console.WriteLine("\n\tStarting the calculation task...");

            // Set the console color back to the original value.
            Console.ForegroundColor = originalcolor;

            // Pause for a moment.
            System.Threading.Thread.Sleep(MillisecondsToPause);

            // Perform the calculation.
            answer = Math.Pow(firstNumber, secondNumber);

            // Pause for another moment.
            System.Threading.Thread.Sleep(MillisecondsToPause);

            // Change the foreground color in the console.
            Console.ForegroundColor = ConsoleColor.Red;

            // Display a message that we're done with the task.
            Console.WriteLine("\n\tDone with the calculation task.");

            // Set the console color back to the original value.
            Console.ForegroundColor = originalcolor;

            return answer;
        }

        // Make this available to code outside of this class.
        internal int MillisecondsToPause
        {
            get { return _millisecondsToPause; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException 
                        ("Milliseconds must be greater than or equal to 0.");
                }

                _millisecondsToPause = value;
            }
        }

        // Provide a way to allow code outside this class to access
        // the results. This is read-only to code outside this class
        // (notice the private accessor on set).
        internal double Results
        {
            get { return _results; }
            private set { _results = value; }
        }
    }

    class Program
    {
        private static int GetMilliseconds(string s)
        {
            int milliseconds = 0;

            // If this call fails, milliseconds will be set to zero.
            if (int.TryParse(s, out milliseconds))
            {
                // If the user types in a low number, let's assume 
                // that they entered in the number of seconds and
                // convert the value to milliseconds.
                if (milliseconds < 250)
                {
                    milliseconds = 1000;
                }
            }

            return milliseconds;
        }

        static void Main(string[] args)
        {
            try
            {
                // Display a message to show we're in Main().
                Console.WriteLine("Starting the program.");

                // Get the number of milliseconds from the arguments 
                // passed in from the command line.
                int milliseconds = GetMilliseconds(args[0]);

                // Create the ComplicatedCalculator object.
                ComplicatedCalculator cc =
                    new ComplicatedCalculator(milliseconds);

                // Create the ParameterizedThreadStart delegate. This 
                // delegate will be used to pass an array of doubles
                // to the method on the secondary thread.
                double[] numbers = { 10.4, 7.451 };
                ParameterizedThreadStart threadedMethod =
                    new ParameterizedThreadStart(cc.CalculateValue);

                // Create the thread object and start the thread. In this
                // case, when we call Start(), we pass in the double array
                // as an argument.
                Thread secondaryThread = new Thread(threadedMethod);
                secondaryThread.Start(numbers);

                // Display some messages to show that Main() is still
                // responsive while the calculation is going on.
                Console.WriteLine
                    ("\nNow I'm going to go do something else.");
                System.Threading.Thread.Sleep(1500);
                Console.WriteLine("\nLike talk about the weather.");
                System.Threading.Thread.Sleep(1500);
                Console.WriteLine("\nOr the latest news.");
                System.Threading.Thread.Sleep(1500);
                Console.WriteLine("\nYou know, my foot hurts.");
                System.Threading.Thread.Sleep(1500);
                Console.WriteLine("\nI love hotdogs!");
                System.Threading.Thread.Sleep(1500);
                Console.WriteLine("\nHow much is a shake at Burgermaster?");
                System.Threading.Thread.Sleep(1500);
                Console.WriteLine("\nOk, now I'm getting hungry!");
                System.Threading.Thread.Sleep(1500);

                // How do we know that we have the answer? At this point
                // we don't. We'll take a chance for now and hope that the
                // answer is there. If the argument passed into Main() is
                // set to 2500, then this will work. If it's set to 7500,
                // then the result will be 0.
                Console.WriteLine("\n\tThe result is: {0}", cc.Results);
            }
            catch (Exception e)
            {
                Console.WriteLine("\nEXCEPTION: {0}.", e.Message);
            }

            // Pause so we can look at the console window.
            Console.Write("\n\nPress <ENTER> to end: ");
            Console.ReadLine();
        }
    }
}
