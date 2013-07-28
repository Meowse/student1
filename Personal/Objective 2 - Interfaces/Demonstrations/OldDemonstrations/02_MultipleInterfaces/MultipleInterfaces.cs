using System;

// Imagine that this interface was created as part of a library
// by a 3rd party vendor. You have no way of modifying the library 
// because you don't own it.
internal interface ITwoDimensional 
{
    // Abstract property for Length.
    float Length
    {
        get;
        set;
    }

    // Abstract property for Width.
    float Width
    {
        get;
        set;
    }

    // Abstract property for Area.
    float Area
    {
        get;
    }
}

// The library does not have a method for calculating 
// the perimeter of a shape. Since you can't update the 
// library, you can create your own interface.
internal interface ITraceable
{
    // Abstract property for the Perimeter.
    float Perimeter
    {
        get;
    }
}

// Your classes are able to implement both interfaces because
// C# allows multiple interface implementation. If ITwoDimensional 
// and ITraceable were classes, we would not be able to do this.
internal class Rectangle : ITwoDimensional, ITraceable
{
    private float lengthInches;
    private float widthInches;

    // Constructor for a box.
    internal Rectangle(float length, float width) 
    {
        lengthInches = length;
        widthInches = width;
    }

    // Interface member implementation.
    public float Length
    {
        get { return lengthInches; }
        set { lengthInches = value; }
    }

    // Interface member implementation.
    public float Width
    {
        get { return widthInches; }
        set { widthInches = value; }
    }

    // Interface member implementation.
    public float Area
    {
        get { return (this.Length * this.Width); }
    }

    // Implementation of the Perimeter property 
    // as declared in the second interface, ITraceable.
    public float Perimeter
    {
        get { return ((this.Length*2) + (this.Width*2)); }
    }
}

class InterfaceExample
{
    static void Main()
    {
        // Declare a class instance myRectangle.
        Rectangle myRectangle = new Rectangle(0.0f, 0.0f);

        // Get the length of the rectangle.
        Console.Write ("Enter the length of the rectangle: ");
        myRectangle.Length = float.Parse(Console.ReadLine());

        // Get the width of the rectangle.
        Console.Write ("Enter the width of the rectangle: ");
        myRectangle.Width = float.Parse(Console.ReadLine());

        // Display the area and perimeter of the rectangle.
        Console.WriteLine("A rectangle with a Length of {0} and a Width of {1} " + 
            "has an Area of {2} and a Perimeter of {3}.", 
            myRectangle.Length,
            myRectangle.Width,
            myRectangle.Area,
            myRectangle.Perimeter);

        Console.Write ("\n\nPress <ENTER> to end: ");
        Console.Read();
    }
}
