using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UsingDelegates
{
    // TODO: Declare a delegate for displaying the menu. The
    //      parameterless method does not return a value. 
    
    internal delegate void CalculateMethod(int inputInt);

    class Calculator
    {
        int _totalValues;

        public void Add(int valueInt)
        {
            _totalValues += valueInt;
        }

        public int TotalValues
        {
            get { return _totalValues; }
        }
    }
    
    class Program
    {
        int _totalValues;

        // TODO: Copy the block of code inside the squiggly braces 
        //      in DisplayMenu method below to be
        //      used in the anonymous method in the next TODO. Then
        //      comment out the entire method below.
        private void DisplayMenu()
        {
            Console.WriteLine("\nCalculator for Add ");
            Console.WriteLine("\n\t1. Add");
            Console.WriteLine("\tX. Exit Calculator");
            Console.Write("\nEnter option: ");
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

        
        static void Main()
        {
            string option = "";
            int valueInt;

            Program p = new Program();
            Calculator c = new Calculator();

            // TODO: Using the delegate you declared above for displaying
            //      a menu, create an anonymous method.  The code for the
            //      anonymous method is the code you copied in the
            //      DisplayMenu method at the previous TODO.
            

            while (option.ToUpper() != "X")
            {
                // TODO:  Call the anonymous method stored in the delegate.
                

                // Get the option from the user.
                option = Console.ReadLine();
                Console.WriteLine();

                switch (option.ToUpper())
                {
                    case "1":
                        // Get numeric value.
                        valueInt = p.GetNumericValue();

                        // Use delegate method to call calculate method.
                        CalculateMethod cm = new CalculateMethod(c.Add);
                        cm(valueInt);

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
