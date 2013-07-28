using System;

// A very simple interface. Notice that none of the properties include 
// access modifiers. All of them are implicitly public. If we included 
// access modifiers, we would see the following compiler error:
//
//     The modifier 'public' is not valid for this item.
//

internal interface ITwoDimensional 
{
    // Abstract property for Length. Notice that for each of the 
    // properties, all that is included here are the accessors that 
    // should be implemented.
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

    // Abstract property for Area. In this case, we are stating 
    // that Area should be a read-only property. However, the 
    // implementing class can provide the set accessor as long 
    // as the get accessor is implemented.
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
    private float depthInches;

    // Construct for a rectangle.
    internal Rectangle(float length, float width) 
    {
        lengthInches = length;
        widthInches = width;
        depthInches = 0;
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

    // We can augment the implementation of the interface 
    // with other methods and properties. Notice the interface
    // does not identify a Depth property.
    internal float Depth
    {
        get {return (depthInches); }
        set { depthInches = value; }
    }

    // Added method for calculating volume. Notice that the
    // interface does not identify a Volume property.
    internal float Volume
    {
        get
        {
            float result = 0;
            if (this.Depth > 0)
            {
                result = (this.Depth * this.Length * this.Width);
            }
            else
            {
                Console.WriteLine ("\n *** Cannot calculate volume on a 2-dimensional rectangle! *** \n");
            }

            return result;
        }
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

        // Get the depth of the rectangle.
        Console.Write ("Enter the depth of the rectangle (0 if not applicable): ");
        myRectangle.Depth = float.Parse(Console.ReadLine());

        Console.WriteLine ();

        // Display the area of the rectangle.
        Console.WriteLine("A rectangle with a Length of {0} and a Width of {1} has an Area of {2}.", 
            myRectangle.Length,
            myRectangle.Width,
            myRectangle.Area);

        // Display the volume of the rectangle.
        Console.WriteLine ("Length: {0}, Width: {1}, Depth: {2}, Volume: {3}.", 
            myRectangle.Length,
            myRectangle.Width,
            myRectangle.Depth,
            myRectangle.Volume);

        Console.Write ("\n\nPress <ENTER> to end: ");
        Console.Read();
    }
}
