using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UsingDelegates
{
    internal delegate void DisplayMenu();
    internal delegate void CalculateMethod();
    internal delegate void CalculationComplete();   // Delegate for callback method.

    class Calculator
    {
        int _totalValues;
        public List<int> values = new List<int>();

        public void Add()
        {
            _totalValues += values[0];
            values.RemoveAt(0);    
        }

        public void Subtract()
        {
            _totalValues -= values[0];
            values.RemoveAt(0);    
        }

        public int TotalValues
        {
            get { return _totalValues; }
        }

        // This method is called with delegates for
        //      both the collection of calculate methods and the callback method.
        public void CallCalculator(CalculateMethod cm, CalculationComplete cc)
        {
            // TODO: Call the multicast CalculateMethod delegate.
            

            // TODO: Call the callback method.
            
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

                        // Load multicast delegate to call Add method.
                        // TODO: Include the Add method in the multicast
                        //      CalculateMethod delegate.

                        
                        // Loads a numeric value into a collection to be used
                        //       by the method when it is executed.
                        c.values.Add(c.GetNumericValue());

                        break;

                    case "2":

                        // Load delegate to call Subtract method.
                        // TODO: Include the Subtract method in the multicast
                        //      CalculateMethod delegate.


                        // Loads a numeric value into a collection to be used
                        //       by the method when it is executed.
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
            //      the multicast CalculateMethod delegate has any calculate methods
            //      loaded into it.
            if (cm == null)
            {
                Console.WriteLine("\nNo calculations were requested.");
            }
            else
            {
                c.CallCalculator(cm, cc);
            }

            Console.WriteLine("\nTotal values: {0}",
                c.TotalValues);

            Console.Write("\nPress any key to end.");
            Console.ReadLine();
        }
    }
}
