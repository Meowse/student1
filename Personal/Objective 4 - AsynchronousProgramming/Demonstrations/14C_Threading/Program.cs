/***************************************************************************
 * 
 * Part 14C: Threading
 * 
 * Topic:  Applying exception handling to secondary threads.
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
            ConsoleColor consoleForegroundColor = Console.ForegroundColor;
            try
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
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine
                    ("\n\t{0}: {1} - The following exception occurred:\n{2}",
                    Thread.CurrentThread.ManagedThreadId,
                    Thread.CurrentThread.Name, ex.Message);
                Console.ForegroundColor = consoleForegroundColor;
            }
        }

        // This method represents a task that could potentially run for
        // a long period of time.
        internal double CalculateValue
            (double firstNumber, double secondNumber)
        {
            double answer = 0;

            // Get the currently-running thread object.
            Thread threadObject = Thread.CurrentThread;

            // Save the foreground color of the console window.
            ConsoleColor originalcolor = Console.ForegroundColor;

            // Change the foreground color in the console.
            Console.ForegroundColor = ConsoleColor.Red;

            // Display a message that we're starting the task.
            Console.WriteLine
                ("\n\t{0}: {1} - Starting the calculation task.",
                threadObject.ManagedThreadId, threadObject.Name);

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
            Console.WriteLine
                ("\n\t{0}: {1} - Done with the calculation task.",
                threadObject.ManagedThreadId, threadObject.Name);

            // Set the console color back to the original value.
            Console.ForegroundColor = originalcolor;

            //Throw an exception.
            throw new ArgumentException("Argument exception raised on secondary thread: " + 
                Thread.CurrentThread.Name + ".");

            // Return the answer. The risk here is that the parent thread
            // may wake up and finish before we can return from here. Not
            // a very good synchronization mechanism.
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

        private static void JoinThread(int mainThreadId, Thread thread)
        {
            // Join the secondary thread, but don't wait forever.
            Console.WriteLine("\n{0}: Joining thread {1}: {2}.",
                mainThreadId, thread.ManagedThreadId, thread.Name);

            if (!thread.Join(10000))
            {
                Console.WriteLine
                    ("\n{0}: Thread {1}: {2} " +
                    "is still alive. Calling Abort().",
                    mainThreadId, thread.ManagedThreadId, thread.Name);

                // If it is, abort the thread and then Join it again.
                thread.Abort();
                if (!thread.Join(10000))
                {
                    Console.WriteLine
                        ("\n{0}: Thread {1}: {2} is still running!",
                        mainThreadId, thread.ManagedThreadId, thread.Name);
                }
            }
        }

        static void Main(string[] args)
        {
            // Get the currently-running thread object.
            Thread primaryThreadObject = Thread.CurrentThread;

            // Set the name of the thread. This will help with debugging
            // when looking at the Threads window.
            primaryThreadObject.Name = "The Main Thread";

            // Get the thread ID so that we can use it in output statements.
            int threadId = primaryThreadObject.ManagedThreadId;

            try
            {
                // Display a message to show we're in Main().
                Console.WriteLine("{0}: Starting the program.", threadId);

                // Get the number of milliseconds from the arguments 
                // passed in from the command line.
                int milliseconds = GetMilliseconds(args[0]);

                // Create the ComplicatedCalculator objects.
                ComplicatedCalculator cc1 =
                    new ComplicatedCalculator(milliseconds);
                ComplicatedCalculator cc2 =
                    new ComplicatedCalculator(milliseconds);

                // Create the ParameterizedThreadStart delegate. This 
                // delegate will be used to pass an array of doubles
                // to the method on the secondary thread.
                double[] numbers1 = { 10.4, 7.451 };
                double[] numbers2 = { 18.7, 3.6 };

                ParameterizedThreadStart threadedMethod1 =
                    new ParameterizedThreadStart(cc1.CalculateValue);
                ParameterizedThreadStart threadedMethod2 =
                    new ParameterizedThreadStart(cc2.CalculateValue);

                // Create the thread objects and start the threads. In this
                // case, when we call Start(), we pass in the double array
                // as an argument.
                Thread secondaryThread1 = new Thread(threadedMethod1);
                Thread secondaryThread2 = new Thread(threadedMethod2);

                // Set the name of the secondary threads.
                secondaryThread1.Name = "Calculation Thread #1";
                secondaryThread2.Name = "Calculation Thread #2";

                // Start the threads.
                secondaryThread1.Start(numbers1);
                secondaryThread2.Start(numbers2);

                // Display some messages to show that Main() is still
                // responsive while the calculation is going on.
                Console.WriteLine
                    ("\n{0}: Now I'm going to go do something else.",
                    threadId);
                System.Threading.Thread.Sleep(1500);
                Console.WriteLine("\n{0}: Like talk about the weather.",
                    threadId);
                System.Threading.Thread.Sleep(1500);
                Console.WriteLine("\n{0}: Or the latest news.",
                    threadId);
                System.Threading.Thread.Sleep(1500);
                Console.WriteLine("\n{0}: You know, my foot hurts.",
                    threadId);
                System.Threading.Thread.Sleep(1500);
                Console.WriteLine("\n{0}: I love hotdogs!",
                    threadId);
                System.Threading.Thread.Sleep(1500);
                Console.WriteLine
                    ("\n{0}: How much is a shake at Burgermaster?",
                    threadId);
                System.Threading.Thread.Sleep(1500);
                Console.WriteLine("\n{0}: Ok, now I'm getting hungry!",
                    threadId);
                System.Threading.Thread.Sleep(1500);

                // Join one of the secondary threads.
                JoinThread(threadId, secondaryThread1);

                // Join the other secondary thread.
                JoinThread(threadId, secondaryThread2);

                Console.WriteLine("\n{0}: The result from {1} is: {2}",
                    threadId, secondaryThread1.ManagedThreadId, cc1.Results);

                Console.WriteLine("\n{0}: The result from {1} is: {2}",
                    threadId, secondaryThread2.ManagedThreadId, cc2.Results);
            }
            catch (Exception e)
            {
                Console.WriteLine("\n{0}: EXCEPTION: {1}.",
                    threadId, e.Message);
            }

            // Pause so we can look at the console window.
            Console.Write("\n\n{0}: Press <ENTER> to end: ",
                threadId);
            Console.ReadLine();
        }
    }
}
