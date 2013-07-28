using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Messaging;

namespace UsingAsyncDelegates
{
    //  Delegate used to call the Add method asynchronously.
    internal delegate int CalculateMethod(int num1, int num2);

    class Calculator
    {
        public int Add(int firstNumber, int secondNumber)
        {
            System.Threading.Thread.Sleep(2000);
            return firstNumber + secondNumber;
        }
    }

    class Program
    {
        int _totalValue;

        //Callback method stored in the callback delegate.
        //  This method runs on the secondary thread.
        private void ComputationComplete(IAsyncResult endAsyncResult)
        {
            AsyncResult ar = (AsyncResult)endAsyncResult;
            CalculateMethod cm = (CalculateMethod)ar.AsyncDelegate;
            TotalValue = cm.EndInvoke(endAsyncResult);
            //TODO: 2. Get beginning time sent from the main thread and store it
            //      in a DateTime variable with the name of beginTime.
            

            Console.WriteLine(
                "\n\n\tThe result is done according to the times below:\n\t\tStart: {0}\n\t\tEnd: {1}\n\n",
                   beginTime.ToLongTimeString(), DateTime.Now.ToLongTimeString());
        }

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

            //Create an instance of the callback delegate passing
            //      in the callback method.
            AsyncCallback callbackMethod =
                new AsyncCallback(p.ComputationComplete);

            //Send the beginning time to the callback method.
            DateTime beginTime = DateTime.Now;

            // Call the Add method stored in the delegate asynchronously.
            // TODO: 1. Include in the asychronous call below the instance of
            //      of the callback delegate (callbackMethod) and the 
            //      variable (beginTime).
            IAsyncResult asyncResult = cm.BeginInvoke(firstNumber, secondNumber, null, null);

            System.Threading.Thread.Sleep(3000);
            
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