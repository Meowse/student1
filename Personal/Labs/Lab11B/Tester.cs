using System;

//TODO: Running instructions:
//      1. After completing the code below and setting the breakpoint, run the application in debugging mode.
//      2. At the breakpoint:
//          a.  Step through the code and then look at the Console window to see the displayed message.
//          b.  Hit the F5 hotkey for the application to finish running.
//          NOTE:  The breakpoint should only occur once for ca2 reference.

namespace DestructorLab
{
    class CheckingAccount : IDisposable
    {
        string _valueString;

        internal CheckingAccount(string inputString)
        {
            _valueString = inputString;
        }

        //TODO: Code the Dispose method to do the following:
        //      1. Display the value in the _valueString variable in the Console window.
        //      2. Prevent the destructor from running.


        //TODO: Code the destructor to do the following:
        //      1. Display the value in the _valueString variable in the Console window.
        //      2. Set a breakpoint on the code line in the destructor.


    }

    class Tester
    {
        static void Main()
        {
            using (CheckingAccount ca1 = new CheckingAccount("Dispose method is called automatically."))
            {
                Console.WriteLine("Destructor will not run as Dispose method will be called automatically.");
            }

            CheckingAccount ca2 = new CheckingAccount("Dispose method is not called causing the Destructor to run.");

            Console.Write("\n\nPress <ENTER> to end: ");
            Console.ReadLine();
        }
    }
}
