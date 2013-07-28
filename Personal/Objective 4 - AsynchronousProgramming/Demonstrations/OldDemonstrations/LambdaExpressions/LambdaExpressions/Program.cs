using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LambdaExpressions
{
    class Program
    {
        delegate double GeometryMath(int[] valueIntegers);

        static void Main(string[] args)
        {
            int[] lengthIntegers = new int[] { 23 };
            //lengthIntegers = null;
            //perimeterCalculation = (int[] perimeterIntegers) =>
            //    { return perimeterIntegers[0] * 3.14; };
            //Console.WriteLine("Circle circumference: " + 
            //    CalculatePerimeters(perimeterCalculation,lengthIntegers));
            Console.WriteLine("Circle circumference: " +
                            CalculatePerimeters((int[] perimeterIntegers) =>
                            { return perimeterIntegers[0] * 3.14; }, lengthIntegers));

            lengthIntegers = new int[] { 14, 45 };
            //perimeterCalculation = (int[] perimeterIntegers) =>
            //{ return perimeterIntegers[0] * 2 + perimeterIntegers[1] * 2; };
            //Console.WriteLine("Square Perimeter: " + 
            //    CalculatePerimeters(perimeterCalculation, lengthIntegers));
            Console.WriteLine("Square Perimeter: " +
                CalculatePerimeters((int[] perimeterIntegers) =>
                { return perimeterIntegers[0] * 2 + perimeterIntegers[1] * 2; }, lengthIntegers));

            lengthIntegers = new int[] { 14, 45, 67, 4, 9, 12 };
            //perimeterCalculation = (int[] perimeterIntegers) =>
            //{
            //    double result = 0.0;
            //    for (int i = 0; i <= perimeterIntegers.GetUpperBound(0); i++)
            //    {
            //        result += perimeterIntegers[i];
            //    }
            //    return result;
            //};
            //Console.WriteLine("Shape perimeter: " +
            //    CalculatePerimeters(perimeterCalculation, lengthIntegers));
            Console.WriteLine("Shape perimeter: " +
                            CalculatePerimeters((int[] perimeterIntegers) =>
                            {
                                double result = 0.0;
                                for (int i = 0; i <= perimeterIntegers.GetUpperBound(0); i++)
                                {
                                    result += perimeterIntegers[i];
                                }
                                return result;
                            }, lengthIntegers));
        }

        static double CalculatePerimeters(GeometryMath gm, int[] valueIntegers)
        {
            if (valueIntegers != null)
                return gm(valueIntegers);
            else
                return 0.0;
        }
    }
}
