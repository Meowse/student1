/******************************************************************************
 * 
 * INSTRUCTIONS:
 * 
 *      Simply locate each TODO comment and implement the code that it
 *      describes. This program does not have an ending operation so after
 *      you run it, you will have to hit CTRL-C to break into it. Then you
 *      can end it by stopping the debugger.
 * 
 *      Let the program run for at least 30 seconds. Then press CTRL-C to
 *      break into the code. From the Debug menu, highlight Windows and click
 *      Threads. Notice how many threads are currently running.
 * 
 ******************************************************************************/

using System;
using System.Threading;

// TODO: Bring in the System.Runtime.Remoting.Messaging namespace.


public class ClassWithDelegate
{
    // TODO: Declare a delegate called DelegateThatReturnsInt which takes
    //       out string argument and returns an int.

    // TODO: Declare and event of type DelegateThatReturnsInt and name it
    //       OnReturnInt.

    public void Run ()
    {
        while (true)
        {
            // Sleep for 1/2 a second.
            Thread.Sleep (500);

            if (null != OnReturnInt)
            {
                foreach (DelegateThatReturnsInt del in OnReturnInt.GetInvocationList())
                {
                    // TODO: Create an AsyncCallback object providing ResultsReturned as
                    //       the method that should be called when the asynchronous
                    //       operation ends.

                    // TODO: Declare a string. Call BeginInvoke() on the delegate that 
                    //       was pulled from the event's Invocation List passing the
                    //       the string as an out argument.
                }
            }
        }
    }

    private void ResultsReturned (IAsyncResult iar)
    {
        // TODO: Convert the IAsyncResult object that was passed in to
        //       an AsyncResult object.

        // TODO: Get the delegate DelegateThatReturnsInt out of the
        //       AsyncResult object by using its AsyncDelegate property.

        // TODO: Declare a string. Call EndInvoke on the delegate providing 
        //       it the string as an out parameters and the IAsyncResult
        //       object. The return type is an integer.

        // TODO: Display the integer result to the console. Display the
        //       string as part of the output.
    }
}

public class FirstSubscriber
{
    private int _myCounter = 0;

    public void Subscribe (ClassWithDelegate cwd)
    {
        // TODO: Subscribe the OnReturnInt event in the ClassWithDelegate object.
    }

    public int DisplayCounter(out string source)
    {
        source = "DisplayCounter";
        Console.WriteLine ("Busy in DisplayCounter() ...");
        Thread.Sleep(10000);
        Console.WriteLine ("Done with work in DisplayCounter()");
        return ++_myCounter;
    }
}

public class SecondSubscriber
{
    private int _myCounter = 0;

    public void Subscribe (ClassWithDelegate cwd)
    {
        // TODO: Subscribe the OnReturnInt event in the ClassWithDelegate object.
    }

    public int Doubler(out string source)
    {
        source = "Doubler";
        return _myCounter += 2;
    }
}

class TestClass
{
    static void Main()
    {
        // TODO: Create an object of type ClassWithDelegate.

        // TODO: Create a FirstSubscriber object and call its Subscribe method.

        // TODO: Create a SecondSubscriber object and call its Subscribe method.

        // TODO: Call the Run method in the ClassWithDelegate class.
    }
}
