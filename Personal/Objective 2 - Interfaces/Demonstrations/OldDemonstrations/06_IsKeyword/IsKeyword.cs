// File: IsKeyword.cs
using System;

internal interface ITwoDimensional 
{
    float Length
    {
        get;
        set;
    }

    float Width
    {
        get;
        set;
    }

    float Area
    {
        get;
    }
}

internal interface ITraceable
{
    float Perimeter
    {
        get;
    }
}

// Notice that class Rectangle does not 
// implement the ITraceable interface.
internal class Rectangle : ITwoDimensional
{
    private float lengthInches;
    private float widthInches;

    internal Rectangle(float length, float width) 
    {
        lengthInches = length;
        widthInches = width;
    }

    public float Length
    {
        get { return lengthInches; }
        set { lengthInches = value; }
    }

    public float Width
    {
        get { return widthInches; }
        set { widthInches = value; }
    }

    public float Area
    {
        get { return (this.Length * this.Width); }
    }
}

class IsKeywordTest
{
    static void Main()
    {
        Rectangle myRectangle = new Rectangle(0.0f, 0.0f);

        // Use the 'is' keyword to find out if myRectangle
        // implements the ITraceable class. If not, the test 
        // will return false.
        if (myRectangle is ITraceable)
        {
            // Since the class implements the ITraceable
            // interface, get an object instance to the
            // interface.
            ITraceable T = (ITraceable)myRectangle;
            Console.Write ("Enter the length of the rectangle: ");
            myRectangle.Length = float.Parse(Console.ReadLine());

            Console.Write ("Enter the width of the rectangle: ");
            myRectangle.Width = float.Parse(Console.ReadLine());

            Console.WriteLine("A rectangle with a Length of {0} and a Width of {1} " + 
                "has an Area of {2} and a Perimeter of {3}.", 
                myRectangle.Length,
                myRectangle.Width,
                myRectangle.Area,
                T.Perimeter);
        }
        else
        {
            Console.WriteLine ("The ITraceable interface is not supported by the Rectangle class.");
        }

        Console.Write ("\n\nPress <ENTER> to end: ");
        Console.ReadLine();
    }
}
