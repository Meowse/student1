using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UsingDelegates
{
    internal delegate void DisplayMenu();
    internal delegate void CalculateMethod(int inputInt);
    //TODO:  Take note of the delegate declared below that will be used
    //      for the callback method.
    internal delegate void CalculationComplete();

    class Calculator
    {
        int _totalValues;

        public void Add(int valueInt)
        {
            _totalValues += valueInt;
        }

        public void Subtract(int valueInt)
        {
            _totalValues -= valueInt;
        }

        public int TotalValues
        {
            get { return _totalValues; }
        }

        // This method is called with delegates for
        //      both the calculate method and the callback method.
        public void CallCalculator(CalculateMethod cm, CalculationComplete cc)
        {
            // Get the input value.
            int inputInt = GetNumericValue();

            // Call the calculate method.
            // TODO: Call the CalculateMethod delegate  to call the 
            //      calculate method that is stored in it passing in the
            //      value in the variable inputInt.
            cm(inputInt);

            // Call the callback method.
            // TODO: Call the CalculationComplete delegate to call
            //      the callback method that is stored in it.
            cc();
        }

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

    }
    
    class Program
    {
        // TODO:  Take note of the method below that will be used
        //      as the callback method to notify the client that the
        //      calculation is complete.
        private void CalculationCompleteCallback()
        {
            Console.WriteLine("\nCalculation is complete.");
        }
        
        static void Main()
        {
            string option = "";

            // TODO:  Take note that the CalculateMethod delegate is being
            //      declared here.  It will be loaded in the TODOs below.  Its
            //      reference name is "cm".
            CalculateMethod cm;

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

            // TODO: Declare the CalculationComplete delegate and load it with
            //      the CalculationCompleteCallback method. Give the reference
            //      name of the delegate "cc".
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

                        // Load delegate method to call Add method.
                        // TODO: Load the already declared CalculateMethod delegate (name is cm)
                        //      with the Add method in the Calculator class.
                        cm = new CalculateMethod(c.Add);

                        // TODO:  Take note that the CallCalculator method in the Calculator class is being
                        //      called here to pass in the two loaded delegates for the calculate
                        //      method and the callback method.
                        c.CallCalculator(cm, cc);

                        break;

                    case "2":

                        // Load delegate method to call Subtract method.
                        // TODO: Load the already declared CalculateMethod delegate (name is cm)
                        //      with the Subtract method in the Calculator class.
                        cm = new CalculateMethod(c.Subtract);

                        // TODO:  Take note that the CallCalculator method in the Calculator class is being
                        //      called here to pass in the two loaded delegates for the calculate
                        //      method and the callback method.
                        c.CallCalculator(cm, cc);

                        break;

                    case "X":   
                        break;

                    default:
                        Console.WriteLine("Menu option {0} is not valid.",
                            option);
                        break;
                }
            }

            Console.WriteLine("\nTotal values: {0}",
                c.TotalValues);

            Console.Write("\nPress any key to end.");
            Console.ReadLine();
        }
    }
}
