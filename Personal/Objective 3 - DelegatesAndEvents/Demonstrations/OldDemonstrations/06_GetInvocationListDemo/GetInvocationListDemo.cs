/***************************************************************************
 * 
 *  This sample demonstrates how to use the GetInvocationList() method to
 *  obtain the collection of sub-delegates from a multicast delegate, and
 *  call each one individually so that the return value of each on can be
 *  evaluated. This is also useful if an exception is thrown in one of the
 *  sub-delegates and it is imperative that the remaining delegates get
 *  called.
 * 
 ***************************************************************************/
using System;

public delegate int Greeting();

public class GetInvocationListTestClass
{
    internal static int Salutation1()
    {
        Console.WriteLine ("\t1. Thank You.");
        return (1);
    }

    internal static int Salutation2()
    {
        Console.WriteLine ("\t2. Good Morning.");
        return (2);
    }

    internal static int Salutation3()
    {
        Console.WriteLine ("\t3. Goodnight.");
        return (3);
    }

    // This method is called to execute the multicate delegate
    // object.
    public static void ExecuteDelegate(Greeting greetings)
    {
        if (null != greetings)
        {
            foreach (Greeting g in greetings.GetInvocationList())
            {
                int result = g();
                Console.WriteLine("\t   Result = {0}.\n", result);
            }
        }
    }

    // Entry point.
    public static void Main()
    {
        //// Notice in each block of code that instead of calling the delegate
        //// directly, it's passed to ExecuteDelegate.
        //Greeting myGreeting = new Greeting(Salutation1);
        //Console.WriteLine ("My single greeting:\n");
        //ExecuteDelegate (myGreeting);

        //Greeting yourGreeting = new Greeting(Salutation2);
        //Console.WriteLine("\nYour single greeting:\n");
        //ExecuteDelegate (yourGreeting);

        //Greeting ourGreeting = myGreeting + yourGreeting;
        //Console.WriteLine("\nOur multicast greeting:\n");
        //ExecuteDelegate (ourGreeting);

        //ourGreeting += new Greeting(Salutation3);
        //Console.WriteLine("\nMulticast greeting including all salutations:\n");
        //ExecuteDelegate (ourGreeting);

        //ourGreeting = ourGreeting - yourGreeting;
        //Console.WriteLine("\nMulticast greeting without your greeting:\n");
        //ExecuteDelegate (ourGreeting);

        //ourGreeting -= myGreeting;
        //Console.WriteLine("\nGreeting without your greeting and my greeting:\n");
        //ExecuteDelegate (ourGreeting);

        Greeting newGreeting = new Greeting(Salutation2);
        newGreeting += new Greeting(Salutation1);
        newGreeting += new Greeting(Salutation3);
        Console.WriteLine("\nNew multicast delegate with different order of methods:\n");
        ExecuteDelegate (newGreeting);

        Console.Write ("\nPress <ENTER> to end: ");
        Console.Read();
    }
}