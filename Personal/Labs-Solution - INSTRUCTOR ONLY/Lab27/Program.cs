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

        // TODO: Create an overloaded Add method that has
        //      one parameter of an object type.  Cast the object
        //      as an int array.  Call the other Add method passing
        //      in the two array elements.
        public void Add(object input)
        {
            int[] intArray = input as int[];
            if (intArray != null && intArray.Length >= 2)
            {
                Add(intArray[0], intArray[1]);
            }
        }


        public void Add(int firstNumber, int secondNumber)
        {
            TotalValue = firstNumber + secondNumber;
          //  System.Threading.Thread.Sleep(2000);
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
            int[] inputValues = new int[2];
            
            Program p = new Program();
            Calculator c = new Calculator();

            // Get first numeric value.
            inputValues[0] = p.GetNumericValue();

            // Get second numeric value.
            inputValues[1] = p.GetNumericValue();

            // TODO: Create an instance of the ParameterizedThreadStart delegate 
            //      passing in the Add method of the Calculator class.
            ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(c.Add);

            // TODO: Declare and create an instance of the secondary thread
            //      passing in the delegate instance.
            Thread thread = new Thread(parameterizedThreadStart);
            //Thread thread = new Thread(c.Add);

            //TODO:  Start the secondary thread passing in the object data.
            thread.Start(inputValues);

            System.Threading.Thread.Sleep(2500);

            Console.WriteLine("\nTotal values: {0}",
                c.TotalValue);

            Console.Write("\nPress any key to end.");
            Console.ReadLine();
        }
    }
}
