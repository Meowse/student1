using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UsingDelegates
{
    internal delegate void DisplayMenu();
    internal delegate string CalculateMethod();
    internal delegate void CalculationComplete();   // Delegate for callback method.

    class Calculator
    {
        int _totalValues;
        public List<int> values = new List<int>();

        public string Add()
        {
            _totalValues += values[0];
            return "add";
        }

        public string Subtract()
        {
            _totalValues -= values[0];
            return "subtract";
        }

        public int TotalValues
        {
            get { return _totalValues; }
        }

        // This method is called with delegates for
        //      both the collection of calculate methods and the callback method.
        public void CallCalculator(CalculateMethod cm, CalculationComplete cc)
        {
            Console.WriteLine("Starting total is: {0}", TotalValues);

            // TODO: Code a foreach loop to loop through the multicast 
            //      CalculateMethod delegate to execute one method at a
            //      time to capture the return value, which is the name of
            //      the calculate method that was called:
            //          Include the commented block of code below in the loop.
            //          Uncomment the block after inserting it into the loop.
            //          Give the reference name "m" to each instance of the
            //              delegate in the foreach loop.
            
            
                //Console.WriteLine("\nResult after {0}ing {1} is {2}.",
                //    m(), values[0], TotalValues);
                //values.RemoveAt(0);
            

            // Call the callback method.
            cc();
        }

        public int GetNumericValue()
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

    }
    
    class Program
    {
        // This is the callback method.
        private void CalculationCompleteCallback()
        {
            Console.WriteLine("\nCalculations are complete.");
        }
        
        static void Main()
        {
            string option = "";

            CalculateMethod cm = null;     // Delegate for calculation methods.

            Program p = new Program();
            Calculator c = new Calculator();

            // Declare an anonymous method.
            DisplayMenu menu = delegate
            {
                Console.WriteLine("\nCalculator for Add and Subtract");
                Console.WriteLine("\n\t1. Add");
                Console.WriteLine("\t2. Subtract");
                Console.WriteLine("\tX. Exit Calculator");
                Console.Write("\nEnter option: ");
            };

            // Declare the CalculationComplete delegate and load it with
            //      the CalculationCompleteCallback method.
            CalculationComplete cc = new CalculationComplete(p.CalculationCompleteCallback);

            while (option.ToUpper() != "X")
            {
                // Call the anonymous method.
                menu();

                // Get the option from the user.
                option = Console.ReadLine();
                Console.WriteLine();

                switch (option.ToUpper())
                {
                    case "1":

                        // Load delegate to call Add method.
                        cm += new CalculateMethod(c.Add);
                        c.values.Add(c.GetNumericValue());

                        break;

                    case "2":

                        // Load delegate to call Subtract method.
                        cm += new CalculateMethod(c.Subtract);
                        c.values.Add(c.GetNumericValue());

                        break;

                    case "X":   
                        break;

                    default:
                        Console.WriteLine("Menu option {0} is not valid.",
                            option);
                        break;
                }
            }

            // Call the method in the Calculator class 
            //      to pass in the two loaded delegates for the calculate
            //      methods and the callback method.  Do this ONLY if
            //      the CalculateMethod delegate has any calculate methods
            //      loaded into it.
            if (cm == null)
            {
                Console.WriteLine("\nNo calculations were requested.");
            }
            else
            {
                c.CallCalculator(cm, cc);
            }

            Console.WriteLine("\nFinal Total is: {0}",
                c.TotalValues);

            Console.Write("\nPress any key to end.");
            Console.ReadLine();
        }
    }
}
