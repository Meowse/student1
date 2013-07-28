using System;
using System.Collections.Generic;
using System.Text;

namespace TestHarness
{
    internal class TestCases
    {
        Random _timeToWaitGenerator;

        internal TestCases(out TestCase testCaseDelegate, int testCaseNumber)
        {
            _timeToWaitGenerator = new Random(DateTime.Now.Millisecond);

            switch (testCaseNumber)
            {
                case 0:
                    testCaseDelegate = new TestCase(RunAllTestCases);
                    break;

                case 1:
                    testCaseDelegate = new TestCase(TestCase1);
                    break;

                case 2:
                    testCaseDelegate = new TestCase(TestCase2);
                    break;

                case 3:
                    testCaseDelegate = new TestCase(TestCase3);
                    break;

                default:
                    Console.WriteLine("WARNING: Invalid case number {0} specified. Defaulting to ALL.", testCaseNumber);
                    testCaseDelegate = new TestCase(RunAllTestCases);
                    break;
            }
        }

        internal bool RunAllTestCases()
        {
            bool bSuccess = true;

            if (bSuccess)
            {
                bSuccess = TestCase1();
            }

            if (bSuccess)
            {
                bSuccess = TestCase2();
            }

            if (bSuccess)
            {
                bSuccess = TestCase3();
            }

            return bSuccess;
        }

        internal bool TestCase1()
        {
            bool bSuccess = true;
            int waitTime = _timeToWaitGenerator.Next(500, 1500);

            Console.WriteLine("\nINFO: Running test case #1.");
            System.Threading.Thread.Sleep(waitTime);
            Console.WriteLine("INFO: Test case #1 took {0} milliseconds to run.\n", waitTime);

            return bSuccess;
        }

        internal bool TestCase2()
        {
            bool bSuccess = true;
            int waitTime = _timeToWaitGenerator.Next(3500, 5000);

            Console.WriteLine("\nINFO: Running test case #2.");
            System.Threading.Thread.Sleep(waitTime);
            Console.WriteLine("INFO: Test case #2 took {0} milliseconds to run.\n", waitTime);

            return bSuccess;
        }

        internal bool TestCase3()
        {
            bool bSuccess = true;
            int waitTime = _timeToWaitGenerator.Next(6000, 10000);

            Console.WriteLine("\nINFO: Running test case #3.");
            System.Threading.Thread.Sleep(waitTime);
            Console.WriteLine("INFO: Test case #3 took {0} milliseconds to run.\n", waitTime);

            Random resultsGenerator = new Random(DateTime.Now.Second);
            int v = resultsGenerator.Next(1, 100);

            // If 'v' is an even number, then the test case failed.
            bSuccess = v%2 == 0 ? false : true;

            return bSuccess;
        }
    }
}
