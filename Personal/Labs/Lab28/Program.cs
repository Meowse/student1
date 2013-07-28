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
        }

        public void Add(int firstNumber, int secondNumber)
        {
            //TODO: Declare a thread to reference the current thread.
            
            
            //TODO: Complete the statement below by including the arguments for
            //      the properties of the thread declared above - thread id and name.
            Console.WriteLine("\n\n\tThread {0}: Add method being executed on {1}.\n",
                );

            TotalValue = firstNumber + secondNumber;
            System.Threading.Thread.Sleep(2000);
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
                Console.Write("\nEnter numeric value to calculate: ");

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
            Calculator c = new Calculator();

            // TODO: Declare a thread to reference the current primary thread.
            //      Make the reference name "primaryThread".
            

            // TODO:  Name the thread just declared.
            

            // TODO:  Store the thread id of the thread declared above in an int
            //      type variable "threadID".
            

            // TODO: complete the command below to include two arguments for
            //      the properties of the thread declared above - thread id and name.
            Console.WriteLine(
                "\nThread {0}-{1}: Now beginning execution.",
                );

            // Create an instance of the ParameterizedThreadStart delegate passing
            //      in the Add method of the Calculator class.
            ParameterizedThreadStart ts = new ParameterizedThreadStart(c.Add);

            // Declare and create an instance of the secondary thread
            //      passing in the delegate instance.
            secondaryThread = new Thread(ts);

            //TODO: Name the secondary thread.
            

            // Get first numeric value.
            valueInts[0] = p.GetNumericValue();

            // Get second numeric value.
            valueInts[1] = p.GetNumericValue();

            //Start the secondary thread passing in the data object.
            secondaryThread.Start(valueInts);

            // TODO: Create a while loop that keeps going as long as
            //      the secondary thread is still going. Include the commented
            //      block of code below in the loop.  After placing the commented
            //      code into the loop, uncomment it.
            
            
                //Console.WriteLine(
                //    "\nThread {0}: Still waiting for secondary thread to complete.", 
                //    primaryThread.ManagedThreadId);
                //System.Threading.Thread.Sleep(750);
            
            
            Console.WriteLine("\nThread {0}: Total values: {1}",
               primaryThread.ManagedThreadId,  c.TotalValue);

            Console.Write("\nPress any key to end.");
            Console.ReadLine();
        }
    }
}
