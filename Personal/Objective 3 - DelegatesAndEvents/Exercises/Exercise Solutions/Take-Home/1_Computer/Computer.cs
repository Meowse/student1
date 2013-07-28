using System;

// This class is called to perform the calculations. However, the
// actual computing implementation is not located in this class. The
// delegate will be used to call the real computing method.
internal class Computer
{
    private int _number;

    // The computing delegate.
    internal delegate double ComputeAnswer(int number);

    internal Computer(int n)
    {
        _number = n;
    }

    // This method will call the delegate.
    internal void GetAnswer(ComputeAnswer theDelegate)
    {
        double answer = 0;

        answer = theDelegate(_number);
        Console.WriteLine ("The answer is: {0}", answer);
    }
}

// This class accepts a number and computes the factorial 
// of than number.
internal class FactorialNumbers
{
    internal double ComputeValue(int number)
    {
        double answer = 0;

        Console.WriteLine ("\nPerforming factorial.");

        // Factorial 0 (!0) is 1.
        if (number == 0)
        {
            answer = 1;
        }
        else if (number > 0)
        {
            answer = 1;
            for (int i=1; i <= number; i++)
            {
                answer *= i;
            }
        }
        else
        {
            answer = -1;
        }

        return answer;
    }
}

// This class will accept a number and square it.
internal class SquareNumbers
{
    internal double ComputeValue(int number)
    {
        Console.WriteLine ("\nPerforming Square.");

        double answer = number * number;
        return answer;
    }
}

internal class TestClass
{
    internal static void EndProgramPrompt()
    {
        Console.Write ("\nPress <ENTER> to end: ");
        Console.Read();
    }

    static void Main()
    {
        int userInput;

        // Prompt the user for a number.
        try
        {
            Console.Write ("Enter a number: ");
            string input = Console.ReadLine();
            userInput = int.Parse (input);
        }
        catch (Exception e)
        {
            Console.WriteLine ("Exception < " + e.Message + " > occurred.");
            TestClass.EndProgramPrompt();
            return;
        }

        // Create the computer object.
        Computer myComputer = new Computer (userInput);

        // Create the objects that contain the computing
        // implementation.
        FactorialNumbers fn = new FactorialNumbers();
        SquareNumbers sn = new SquareNumbers();

        // Create the delegate objects providing the computing methods
        // that have been implemented.
        Computer.ComputeAnswer fnDelegate = new Computer.ComputeAnswer (fn.ComputeValue);
        Computer.ComputeAnswer snDelegate = new Computer.ComputeAnswer (sn.ComputeValue);

        // Perform the calculations.
        myComputer.GetAnswer(fnDelegate);
        myComputer.GetAnswer(snDelegate);

        Console.WriteLine();
        TestClass.EndProgramPrompt();
    }
}

