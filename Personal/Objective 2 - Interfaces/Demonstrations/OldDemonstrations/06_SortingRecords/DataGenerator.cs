// DataGenerator.cs
using System;
using System.Text;
using System.Collections;

namespace SortingRecords
{
    internal class DataGenerator
    {
        // This random number generator will be used to generate the data
        // needed by the client code.
        private Random randomizer;

        internal DataGenerator()
        {
            if (null == randomizer)
            {
                // Create a random number generator for the list elements. 
                // The seed is based on the millisecond value of the 
                // current time added to a prime number.
                randomizer = new Random(DateTime.Now.Millisecond + 1009);
            }
        }

        // Gets a number between minValue and maxValue.
        internal int GetPositiveNumber(int minValue, int maxValue)
        {
            int result = GetRandomNumber(minValue, maxValue);
            return result;
        }

        // Get a string of randomly-generated characters. The second argument
        // indicates whether or not the first letter in the string should be
        // capitalized.
        internal string GetString(int size, bool bCapitalizeFirstLetter)
        {
            bool bSuccess = true;   // Used to force a single exit point.
            string s = "";

            // Check to make sure the size of the array being passed in 
            // is valid.
            if (bSuccess)
            {
                if (size <= 0)
                {
                    Console.WriteLine 
                        ("\nDATAGENERATOR: String size cannot be <= zero.");
                    bSuccess = false;
                }
            }

            // Generate the string of random characters.
            if (bSuccess)
            {
                int nextLetterIndex = 0;

                // Does the client want the first letter to be capitalized?
                if (bCapitalizeFirstLetter)
                {
                    // Create the first letter of the string from the 
                    // capital letters in the ASCII table.
                    s = (Convert.ToChar(GetRandomNumber(65, 90))).ToString();
                    nextLetterIndex++;
                }

                // Create the remaining letters of the string.
                for (int i = nextLetterIndex; i < size; i++)
                {
                    char c = Convert.ToChar(GetRandomNumber(97, 122));
                    s = s + c.ToString();
                }
            }

            return s;
        }

        // Gets the next random number based on the minimum and maximum
        // values passed in.
        private int GetRandomNumber (int minValue, int maxValue)
        {
            // Use a flag to force a single exit point.
            bool bSuccess = true;

            int result = -1;

            // Check to see if the minValue is a positive number.
            if (bSuccess)
            {
                if (minValue <= 0)
                {
                    Console.WriteLine 
                        ("\nDATAGENERATOR: The minimum value provided " +
                        "must be greater than 0.");
                    bSuccess = false;
                }
            }

            // Check to see that minValue is less than maxValue.
            if (bSuccess)
            {
                if (minValue >= maxValue)
                {
                    Console.WriteLine ("\nDATAGENERATOR: The minimum " +
                        "value {0} has a greater magnitude " +
                        "than the maximum value of {1}.", 
                        minValue, maxValue);
                    bSuccess = false;
                }
            }

            // If the random number generator happens to return a negative
            // number, call the Absolute Value method in the Math class to
            // create a positive number out of it.
            if (bSuccess)
            {
                result = randomizer.Next(minValue, maxValue);
                if (result < 0)
                {
                    Console.WriteLine ("\nDATAGENERATOR: Result of {0} " +
                        "will be converted to a positive number.",
                        result);

                    result = Math.Abs(result);
                }
            }

            return result;
        }
    }
}
