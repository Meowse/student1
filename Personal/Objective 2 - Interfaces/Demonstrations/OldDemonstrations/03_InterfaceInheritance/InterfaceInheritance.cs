using System;

// A very simple interface.
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

// A second interface inheriting from the first.
internal interface ITraceable : ITwoDimensional
{
    // Abstract property for the Perimeter.
    float Perimeter
    {
        get;
    }
}

// A class that implements the ITraceable interface.
// Even though this class inherits only from ITraceable, 
// it must implement the members of ITwoDimensional because 
// ITraceable inherited from IDimension.
internal class Rectangle : ITraceable
{
    private float lengthInches;
    private float widthInches;

    internal Rectangle(float length, float width) 
    {
        lengthInches = length;
        widthInches = width;
    }

    // Having the following three methods commented out will produce these 
    // errors:
    //
    //     'Rectangle' does not implement interface member 'ITwoDimensional.Area'
    //     'Rectangle' does not implement interface member 'ITwoDimensional.Length'
    //     'Rectangle' does not implement interface member 'ITwoDimensional.Width'
    //
    // These interface members must be implemented because ITraceable extended
    // ITwoDimensional by adding on the Perimeter member. The three members in
    // ITwoDimensional still need to be implemented as well.
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

    //public float Area
    //{
    //    get { return (this.Length * this.Width); }
    //}

    public float Perimeter
    {
        get { return ((this.Length*2) + (this.Width*2)); }
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
