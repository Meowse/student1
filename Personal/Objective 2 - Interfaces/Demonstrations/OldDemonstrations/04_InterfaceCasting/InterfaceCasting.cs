using System;

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

internal interface ITraceable
{
    // Abstract property for the Perimeter.
    float Perimeter
    {
        get;
    }
}

internal class Rectangle : ITwoDimensional, ITraceable
{
    private float lengthInches;
    private float widthInches;

    // Constructor for a rectangle.
    public Rectangle(float length, float width) 
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
    private static void PrintArea (ITwoDimensional iDim)
    {
        Console.WriteLine ("\nArea of current shape is {0}", iDim.Area);
    }

    static void Main()
    {
        // Declare a class instance myRectangle.
        Rectangle myRectangle = new Rectangle(0.0f, 0.0f);

        // Create two interface instances, one for each 
        // interface that the class Rectangle implements.
        // If you view the Locals window while stepping
        // through in debug mode, you will see that the 
        // values assigned to id and it are of type
        // {Rectangle}.
        ITwoDimensional id = (ITwoDimensional) myRectangle;
        ITraceable it = (ITraceable) myRectangle;

        // Get the length of the rectangle.
        Console.Write ("Enter the length of the rectangle: ");
        id.Length = float.Parse(Console.ReadLine());

        // Get the width of the rectangle.
        Console.Write ("Enter the width of the rectangle: ");
        id.Width = float.Parse(Console.ReadLine());

        // Display the area and perimeter of the rectangle.
        Console.WriteLine("A rectangle with a Length of {0} and a Width of {1} " + 
            "has an Area of {2} and a Perimeter of {3}.", 
            id.Length,
            id.Width,
            id.Area,
            it.Perimeter);

        // Notice how we can pass a Rectangle object to
        // PrintArea. This is done via polymorphism. Any
        // class that implements the ITwoDimensional interface
        // could be passed into PrintArea.
        PrintArea(myRectangle);

        Console.Write ("\n\nPress <ENTER> to end: ");
        Console.Read ();
    }
}
