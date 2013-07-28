using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UsingAsyncDelegates
{
    // Delegate to be used to call the Add method asynchronously.
    internal delegate 
        int CalculateMethod(int num1, int num2);

    class Calculator
    {
        public int Add(int firstNumber, int secondNumber)
        {
            return firstNumber + secondNumber;
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
            int totalValue = 0;
            int firstNumber;
            int secondNumber;

            Program p = new Program();
            Calculator c = new Calculator();

            // Get first numeric value.
            firstNumber = p.GetNumericValue();

            // Get second numeric value.
            secondNumber = p.GetNumericValue();

            //Create an instance of the delegate passing in
            //      the Add method of the Calculator class.
            CalculateMethod cm = new CalculateMethod(c.Add);

            // TODO: Call the CalculateMethod delegate, named "cm", asynchronously.
            IAsyncResult result = cm.BeginInvoke(firstNumber, secondNumber, null, null);

            // TODO: Capture the return value into the variable "totalValue" (declared above)
            //      from the asynchronous call to the CalculateMethod, named "cm".
            totalValue = cm.EndInvoke(result);

            Console.WriteLine("\nTotal values: {0}",
                totalValue);

            Console.Write("\nPress any key to end.");
            Console.ReadLine();
        }
    }
}
