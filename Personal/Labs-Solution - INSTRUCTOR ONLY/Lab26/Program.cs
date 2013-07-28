using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// TODO: Insert a using directive that is needed for working
//      with the thread classes and delegates.
using System.Threading;

namespace UsingThreads
{
    class Calculator
    {
        private int _firstNumber;
        private int _secondNumber;
        private int _totalValue;

        // Method to be called by the ThreadStart delegate.
        public void Add()
        {
            TotalValue = FirstNumber + SecondNumber;
            System.Threading.Thread.Sleep(2000);
        }

        public int FirstNumber
        {
            get { return _firstNumber; }
            set { _firstNumber = value; }
        }

        public int SecondNumber
        {
            get { return _secondNumber; }
            set { _secondNumber = value; }
        }

        public int TotalValue
        {
            get { return _totalValue; }
            set { _totalValue = value; }
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
            Program p = new Program();
            Calculator c = new Calculator();

            // Get first numeric value.
            c.FirstNumber = p.GetNumericValue();

            // Get second numeric value.
            c.SecondNumber = p.GetNumericValue();

            // TODO: Create an instance of the ThreadStart delegate passing
            //      in the Add method of the Calculator class.
            ThreadStart threadStart = new ThreadStart(c.Add);
                        
            // TODO: Declare and create an instance of the secondary thread
            //      passing in the delegate instance.
            Thread thread = new Thread(threadStart);

            //TODO:  Start the secondary thread.
            thread.Start();

            System.Threading.Thread.Sleep(2500);
            
            Console.WriteLine("\nTotal values: {0}",
                c.TotalValue);

            Console.Write("\nPress any key to end.");
            Console.ReadLine();
        }
    }
}