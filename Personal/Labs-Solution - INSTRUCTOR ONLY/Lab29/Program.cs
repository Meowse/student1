using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace UsingThreads
{
    class Calculator
    {
        private int _totalValue;
        private Thread _primaryThread;

        // Constructor gets a reference to the primary thread
        //      to interrupt it when the time is ready.
        public Calculator(Thread primaryThread)
        {
            PrimaryThread = primaryThread;
        }

        // Method called using the ParameterizedThreadStart delegate.
        public void Add(object inputValue)
        {
            int[] inputValues = (int[])inputValue;
            Add(inputValues[0], inputValues[1]);

            Console.WriteLine(
                "\n\tThread {0}: Secondary thread waking up the primary thread.",
                Thread.CurrentThread.ManagedThreadId);

            // TODO: Interrupt the primary thread.
            PrimaryThread.Interrupt();
        }

        public void Add(int firstNumber, int secondNumber)
        {
            Thread secondaryThread = Thread.CurrentThread;
            Console.WriteLine("\n\n\tThread {0}: Add method being executed on {1}.\n",
                secondaryThread.ManagedThreadId, secondaryThread.Name);

            TotalValue = firstNumber + secondNumber;
            System.Threading.Thread.Sleep(2000);
        }

        public int TotalValue
        {
            get { return _totalValue; }
            private set { _totalValue = value; }
        }

        private Thread PrimaryThread
        {
            get { return _primaryThread; }
            set { _primaryThread = value; }
        }
    }

    class Program
    {
        private int GetNumericValue()
        {
            bool numericValue = false;
            int valueInt = 0;

            // Loop until input value is numeric.
            while (!numericValue)
            {
                // Enter value to calculate.
                Console.Write("\nEnter numeric value to add: ");

                // Convert input value to a numeric value.
                numericValue = int.TryParse(Console.ReadLine(), out valueInt);
                Console.WriteLine();
                if (numericValue)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Input value not numeric.  Enter again.");
                }
            }

            return valueInt;
        }
        
        static void Main()
        {
            int[] valueInts= new int[2];
            Thread secondaryThread = null;

            Program p = new Program();

            // Declare a thread to reference the current primary thread,
            //      name it, and place its id into a variable.
            Thread primaryThread = Thread.CurrentThread;
            primaryThread.Name = "The Main Thread";
            int threadID = primaryThread.ManagedThreadId;

            Calculator c = new Calculator(primaryThread);

            Console.WriteLine(
                "\nThread {0}-{1}: Now beginning execution.",
                primaryThread.ManagedThreadId, primaryThread.Name);

            // Create an instance of the ParameterizedThreadStart delegate passing
            //      in the Add method of the Calculator class.
            ParameterizedThreadStart ts = new ParameterizedThreadStart(c.Add);

            // Declare and create an instance of the secondary thread
            //      passing in the delegate instance.
            secondaryThread = new Thread(ts);
            // Name the secondary thread.
            secondaryThread.Name = "The Secondary Thread";

            // Get first numeric value.
            valueInts[0] = p.GetNumericValue();

            // Get second numeric value.
            valueInts[1] = p.GetNumericValue();

            //Start the secondary thread passing in the data object.
            secondaryThread.Start(valueInts);

            //TODO:  Put the primary thread to sleep infinitely in a 
            //      try block and then use the catch block to catch
            //      the exception raised when threads are interrupted.
            //      NOTE: No code is necessary in the catch block.
            try
            {
                Console.WriteLine("\nThread {0}: Putting main to sleep indefinitely.", 
                    primaryThread.ManagedThreadId);
                Thread.Sleep(Timeout.Infinite);
            }
            catch (ThreadInterruptedException)
            {
                // No code needed here.
            }
            
            Console.WriteLine("\nThread {0}: Total value: {1}",
               primaryThread.ManagedThreadId,  c.TotalValue);

            Console.Write("\nPress any key to end.");
            Console.ReadLine();
        }
    }
}
