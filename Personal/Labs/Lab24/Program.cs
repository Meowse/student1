// TODO:  Use the 06_AsyncDelegates demonstration as a
//      guide for completing the TODOs below in this lab.
//      This lab has only 10 minutes to complete.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//TODO: Insert the using directive that is required for capturing
//          the return value in the callback method.


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
        int _totalValue;

        //TODO:  Code the callback method to capture the return value
        //      from the asynchronous call and store it in the TotalValue
        //      property.  After the return value is captured, display 
        //      a message on the console that the calculation is complete.

        

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

            //TODO:  Create an instance of the callback delegate passing
            //      in the callback method you implemented above.
            

            // Call the Add method stored in the delegate asynchronously.
            //TODO:  Include the instance of the callback delegate in the
            //      call below.
            IAsyncResult asyncResult = cm.BeginInvoke(firstNumber, secondNumber, null, null);

            System.Threading.Thread.Sleep(2500);

            Console.WriteLine("\nTotal value: {0}",
                p.TotalValue);

            Console.Write("\nPress any key to end.");
            Console.ReadLine();
        }

        private int TotalValue
        {
            get { return _totalValue; }
            set { _totalValue = value; }
        }
    }
}
