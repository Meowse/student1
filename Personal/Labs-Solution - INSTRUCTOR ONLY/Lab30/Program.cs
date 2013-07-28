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

        // Method called using the ParameterizedThreadStart delegate.
        public void Add(object inputValue)
        {
            int[] inputValues = (int[])inputValue;
            Add(inputValues[0], inputValues[1]);

            Console.WriteLine(
                "\n\tThread {0}: Secondary thread is done.",
                Thread.CurrentThread.ManagedThreadId);
        }

        public void Add(int firstNumber, int secondNumber)
        {
            Thread secondaryThread = Thread.CurrentThread;
            Console.WriteLine("\n\n\tThread {0}: Add method being executed on {1}.\n",
                secondaryThread.ManagedThreadId, secondaryThread.Name);

            System.Threading.Thread.Sleep(10000);
            TotalValue = firstNumber + secondNumber;
        }

        public int TotalValue
        {
            get { return _totalValue; }
            private set { _totalValue = value; }
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

            Calculator c = new Calculator();

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

            //TODO: Join the secondary thread to the primary thread
            //      for two seconds.  If secondary thread does not complete
            //      within 2 seconds, then do the following:
            //      a.  Display a message on the console that the thread is
            //              is still alive and being aborted.
            //      b.  Abort the secondary thread.
            //      c.  Join again for another 2 seconds to make sure thread
            //              is aborted.
            Console.WriteLine("\nThread {0}: Joining to secondary thread.",
               primaryThread.ManagedThreadId);
            if (!secondaryThread.Join(2000))
            {
                Console.WriteLine("\nThread {0}: Aborting secondary thread.",
                   primaryThread.ManagedThreadId);
                secondaryThread.Abort();
                //for (int secondsCount = 0; secondsCount < 2; secondsCount++)
                //{
                //    Console.WriteLine("\nThread {0}: Secondary thread still alive",
                //       primaryThread.ManagedThreadId);
                //    Thread.Sleep(1000);
                //}
                if (!secondaryThread.Join(2000))
                {
                    Console.WriteLine("\nThread {0}: Secondary thread still alive.",
                       primaryThread.ManagedThreadId);
                }

                return;
            }
            else
            {
                Console.WriteLine("\nThread {0}: Total value: {1}",
                   primaryThread.ManagedThreadId,  c.TotalValue);
            }

            Console.Write("\nPress any key to end.");
            Console.ReadLine();
        }
    }
}
