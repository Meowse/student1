﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UsingAsyncDelegates
{
    //  Delegate used to call the Add method asynchronously.
    internal delegate int CalculateMethod(int num1, int num2);

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

            // Use delegate method to call the Add method.
            CalculateMethod cm = new CalculateMethod(c.Add);

            // Call the Add method asynchronously.
            IAsyncResult asyncResult = cm.BeginInvoke(firstNumber, secondNumber, null, null);

            //TODO:  Enclose the following code into a loop that is polling to
            //          determine when the asynchronous call is done.  After 
            //          including the two lines of code below in the loop, then
            //          uncomment them.
            while (!asyncResult.IsCompleted)
            {
                System.Threading.Thread.Sleep(750);
                Console.WriteLine("\nSeeing if the asynchronous call is done.");
            }


            // Retrieve the return value from the Add method.
            totalValue = cm.EndInvoke(asyncResult);

            Console.WriteLine("\nTotal value: {0}",
                totalValue);

            Console.Write("\nPress any key to end.");
            Console.ReadLine();
        }
    }
}
