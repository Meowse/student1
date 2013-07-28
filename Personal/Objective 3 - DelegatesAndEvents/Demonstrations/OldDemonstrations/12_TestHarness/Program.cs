using System;
using System.Collections.Generic;
using System.Text;

namespace TestHarness
{
    // This delegate will wrap the method that represents 
    // a test case in this system.
    internal delegate bool TestCase ();

    public class Harness
    {
        // The command-line will provide which test case to run.
        private static int _testCaseNumber = -1;

        static void Main(string[] args)
        {
            bool testResult = true;

            try
            {
                // Only one argument should be specified on the command-line, the
                // number of the test case to run.
                if (!ProcessArguments(args))
                {
                    throw new ApplicationException("Could not process incoming command-line arguments.");
                }

                // Declare a reference to the delegate.
                TestCase testCase;

                // Create the test cases object providing the delegate and which
                // test case(s) to run.
                TestCases testCases = new TestCases(out testCase, _testCaseNumber);

                // Run the test case(s).
                testResult = testCase();

                // Check the results.
                if (!testResult)
                {
                    throw new ApplicationException("One or more test cases failed.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\nEXCEPTION: {0}\n", e.Message);
                Console.WriteLine("             {0}", e.StackTrace);
            }
            finally
            {
                Console.Write("\nPress <ENTER> to end: ");
                Console.ReadLine();
            }
        }

        private static bool ProcessArguments(string[] arguments)
        {
            bool bSuccess = true;

            // Verify that only 1 argument was provided on the command-line.
            if (bSuccess)
            {
                if (arguments.Length != 1)
                {
                    bSuccess = false;
                    Console.WriteLine("ERROR: Invalid number of command-line arguments provided.");
                }
            }

            // Process the single argument.
            if (bSuccess)
            {
                try
                {
                    _testCaseNumber = int.Parse(arguments[0]);
                }
                catch (Exception e)
                {
                    Console.WriteLine("EXCEPTION: {0}", e.Message);
                    Console.WriteLine("           {0}", e.StackTrace);
                    bSuccess = false;
                }
            }

            return bSuccess;
        }
    }
}
