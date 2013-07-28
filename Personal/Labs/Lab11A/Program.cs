using System;

namespace DestructorLab
{
    //TODO: Implement the IDisposable interface.
    class CheckingAccount
    {
        string _valueString;

        internal CheckingAccount(string inputString)
        {
            _valueString = inputString;
        }

        //TODO: Code the Dispose method to do the following:
        //      1. Display the value in the _valueString variable in the Console window.

    }

    class Tester
    {
        static void Main()
        {
            // TODO: Implement the "using" block to do the 
            //      following:
            //          1. Declare and create an instance of a CheckingAccount
            //              passing in the message 
            //              "Dispose method is called automatically.".
            //          2. Display the message on the Console that the CheckingAccount
            //              instance is being processed in a using block.


            //TODO: Create a separate instance of the CheckingAccount outside of a
            //          "using" block passing in the message
            //          "Dispose method is being called explicitly.".

            //TODO: Call the Dispose method of the just created instance of the
            //          CheckingAccount.


            Console.Write("\n\nPress <ENTER> to end: ");
            Console.ReadLine();
        }
    }
}