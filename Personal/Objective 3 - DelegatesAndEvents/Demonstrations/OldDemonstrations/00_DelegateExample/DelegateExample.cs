using System;

namespace DelegateExample
{
    // TODO: Declare a delegate named DoubleOperation that takes
    //       a single argument of type double.

    class MainClass
    {
        private static double MultiplyByTwo(double val)
        {
            return val * 2;
        }

        private static double DivideByFive(double val)
        {
            return val / 5;
        }

        private static double SquareNumber(double val)
        {
            return val * val;
        }

        private static double Divide100ByValue(double val)
        {
            // Although doubles can handle divide-by-zero, our solution
            // won't allow it here.
            if (0 == val)
            {
                throw new ArgumentException("Cannot divide by 0!");
            }

            return 100 / val;
        }

        static void Main()
        {
            try
            {
                // TODO: Create an object of type DoubleOperation providing
                //       MultipleByTwo as the method to call.
                Console.WriteLine("{0}\n", MultiplyByTwo(4));

                // TODO: Create an object of type DoubleOperation providing
                //       DivideByFive as the method to call.
                Console.WriteLine("{0}\n", DivideByFive(75));

                // TODO: Create an object of type DoubleOperation providing
                //       SquareNumber as the method to call.
                Console.WriteLine("{0}\n", SquareNumber(100));

                // TODO: Create an object of type DoubleOperation providing
                //       Divide100ByValue as the method to call.
                Console.WriteLine("{0}\n", Divide100ByValue(0));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.Write("\nPress <ENTER> to end: ");
            Console.ReadLine();
        }
    }
}