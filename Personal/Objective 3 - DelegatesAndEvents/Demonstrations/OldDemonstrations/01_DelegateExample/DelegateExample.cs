using System;

namespace DelegateExample
{
    // TODO: Declare a delegate named DoubleOperation that takes
    //       a single argument of type double.
    internal delegate double DoubleOperation(double x);

    class MainClass
    {
        // The following four methods are PROPER methods, meaning they
        // can be called directly, or through the delegate declared
        // above. This is possible because the signatures of these
        // methods matches the signature of the delegate.
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
                DoubleOperation d = new DoubleOperation(MultiplyByTwo);
                Console.WriteLine("{0}\n", d(4));
                //Console.WriteLine("{0}\n", MultiplyByTwo(4))

                // TODO: Create an object of type DoubleOperation providing
                //       DivideByFive as the method to call.
                d = new DoubleOperation(DivideByFive);
                Console.WriteLine("{0}\n", d(75));
                //Console.WriteLine("{0}\n", DivideByFive(75));

                // TODO: Create an object of type DoubleOperation providing
                //       SquareNumber as the method to call.
                d = new DoubleOperation(SquareNumber);
                Console.WriteLine("{0}\n", d(100));
                //Console.WriteLine("{0}\n", SquareNumber(100));

                // TODO: Create an object of type DoubleOperation providing
                //       Divide100ByValue as the method to call.
                d = new DoubleOperation(Divide100ByValue);
                Console.WriteLine("{0}\n", d(0));
                //Console.WriteLine("{0}\n", Divide100ByValue(0));
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