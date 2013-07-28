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

// A class that implements the interface.
internal class Rectangle : ITwoDimensional 
{
    private float lengthInches;
    private float widthInches;

    // Construct for a rectangle.
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

    // Explicit implementation for the Area property.
    float ITwoDimensional.Area
    {
        get { return (this.Length * this.Width); }
    }
}

class InterfaceExample
{
    static void Main()
    {
        Rectangle myRectangle = new Rectangle(0.0f, 0.0f);

        Console.Write ("Enter the length of the rectangle: ");
        myRectangle.Length = float.Parse(Console.ReadLine());

        Console.Write ("Enter the width of the rectangle: ");
        myRectangle.Width = float.Parse(Console.ReadLine());

        // The following line generate an error: 'Rectangle' does not
        // contain a definition for 'Area'.
//        Console.WriteLine("A rectangle with a Length of {0} and a Width of {1} has an Area of {2}.", 
//            myRectangle.Length,
//            myRectangle.Width,
//            myRectangle.Area);

        // To resolve the error, the following code is needed.
        ITwoDimensional id = myRectangle as ITwoDimensional;
        if (id != null)
        {
            Console.WriteLine("A rectangle with a Length of {0} and a Width of {1} has an Area of {2}.", 
                myRectangle.Length,
                myRectangle.Width,
                id.Area);
        }

        Console.Write ("\n\nPress <ENTER> to end: ");
        Console.ReadLine();
    }
}
