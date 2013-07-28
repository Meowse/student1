/***************************************************************************
 * 
 *  This program demonstrates how to use a multicast delegate. This program
 *  also shows that the referenced methods are called in the order in which
 *  they were added to the delegate object. This is a great feature if
 *  you need to modify the order of the methods depending on the
 *  circumstances.
 * 
 ***************************************************************************/

using System;

// Define our multicast delegate and call it Greeting. Notice that the
// return type of this delegate is void. By default, this delegates is 
// defined as a multicast delegate, even if it encapsulates only one
// method.
public delegate void Greeting();

public class MulticastDelegateTestClass
{
    // The following three methods will be encapsulated by
    // the Greeting delegate during this program.
    internal static void Salutation1 ()
    {
        Console.WriteLine ("\t1. Thank You.");
    }

    internal static void Salutation2()
    {
        Console.WriteLine ("\t2. Good Morning.");
    }

    internal static void Salutation3()
    {
        Console.WriteLine ("\t3. Goodnight.");
    }

    // Entry point.
    public static void Main()
    {
        // Create a delegate object of type Greeting and have it
        // encapsulate a single method - Salutation1().
        Greeting myGreeting = new Greeting(Salutation1);
        Console.WriteLine ("My single greeting: ");
        myGreeting();

        // Create another delegate object of type Greeting and have
        // it encapsulate a single method - Salutation2().
        Greeting yourGreeting = new Greeting(Salutation2);
        Console.WriteLine ("\nYour single greeting: ");
        yourGreeting();

        // Create a multicast delegate of type Greeting and have
        // it encapsulate two methods. These methods are represented
        // by delegate objects already. Notice the use of the '+'
        // operator to create a multicast delegate.
        Greeting ourGreeting = myGreeting + yourGreeting;
        Console.WriteLine ("\nOur multicast greeting: ");
        ourGreeting();

        // Continue to use the multicast delegate and add another 
        // encapsulated method to it - Salutation3. Now the
        // multicast delegate encapsulates 3 methods. Notice the
        // use of the '+=' operator. If we would have used just
        // the '=' operator, the already referenced methods in the
        // multicast delegate would be unreferenced and replaced by
        // the new method we're assigning here.
        ourGreeting += new Greeting(Salutation3);
        Console.WriteLine ("\nMulticast greeting with all salutations: ");
        ourGreeting();

        // Remove one of the encapsulated methods from the multicast
        // delegate - the one represented by the yourGreeting delegate
        // object. Notice the use of the '-' operator.
        ourGreeting = ourGreeting - yourGreeting;
        Console.WriteLine ("\nMulticast greeting without your greeting: ");
        ourGreeting();

        // Remove another encapsulated method from the multicast
        // delegate - the one represented by the myGreeting delegate
        // object. Notice the use of the '-=' operator.
        ourGreeting -= myGreeting;
        Console.WriteLine ("\nGreeting without your greeting and my greeting: ");
        ourGreeting();

        // Now create a multicast delegate to show that it will
        // call each method in the order it was encapsulated by the
        // delegate object.
        Greeting newGreeting = new Greeting(Salutation2);
        newGreeting += new Greeting(Salutation1);
        newGreeting += new Greeting(Salutation3);
        Console.WriteLine ("\nNew multicast delegate with different order of methods:");
        newGreeting();

        Console.Write ("\n\nPress <ENTER> to end: ");
        Console.Read();
    }
}