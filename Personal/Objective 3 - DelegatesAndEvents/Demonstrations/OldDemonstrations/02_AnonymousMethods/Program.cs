/****************************************************************************
 * 
 *  This simple console program shows how to use anonymous methods. Rather 
 *  than creating a separate method and then providing its name to a new 
 *  delegate object, we create a delegate object that contains the code 
 *  of the target method internally.
 * 
 ***************************************************************************/

using System;

namespace AnonymousMethods
{
    internal delegate double DoubleOperation (double x);

    class MainClass
    {
        //// The following four methods are PROPER methods, meaning they
        //// can be called directly, or through the delegate declared
        //// above. This is possible because the signatures of these
        //// methods matches the signature of the delegate.
        //private static double MultiplyByTwo(double val)
        //{
        //    return val * 2;
        //}

        //private static double DivideByFive(double val)
        //{
        //    return val / 5;
        //}

        //private static double SquareNumber(double val)
        //{
        //    return val * val;
        //}

        //private static double Divide100ByValue(double val)
        //{
        //    // Although doubles can handle divide-by-zero, our solution
        //    // won't allow it here.
        //    if (0 == val)
        //    {
        //        throw new ArgumentException("Cannot divide by 0!");
        //    }

        //    return 100 / val;
        //}

        static void Main()
        {
            // Create four delegate objects with anonymous methods. Rather
            // than having four PROPER methods, the anonymous methods are
            // coded when writing the code to create the delegate objects.
            // The limitation is that the only way the anonymous methods 
            // can be called is via their delegate.
            DoubleOperation MultiplyByTwo = 
                delegate (double val) { return val * 2; };

            DoubleOperation DivideByFive = 
                delegate (double val) { return val / 5; };

            DoubleOperation SquareNumber = 
                delegate (double val) { return val * val; };

            DoubleOperation Divide100ByValue = delegate(double val)
            {
                // Although doubles can handle divide-by-zero, our solution
                // won't allow it here.
                if (0 == val)
                {
                    throw new ArgumentException("Cannot divide by 0!");
                }

                return 100 / val;
            };

            try
            {
                // The differences between this demonstration and the 
                // previous demonstration are:
                //   1. We aren't creating instances of delegate objects
                //      here. They were already created above.
                //   2. There are no proper methods. All code in the
                //      methods are now part of creating the delegate
                //      objects above.
                Console.WriteLine("{0}\n", MultiplyByTwo(4));
                Console.WriteLine("{0}\n", DivideByFive(75));
                Console.WriteLine("{0}\n", SquareNumber(100));
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
