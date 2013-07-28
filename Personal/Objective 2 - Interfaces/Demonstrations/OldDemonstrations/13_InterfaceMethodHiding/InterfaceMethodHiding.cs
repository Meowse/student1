using System;

internal interface ISortable
{
    void Sort ();
}

internal interface IOrderable
{
    void Sort ();
}

internal interface IStackable : ISortable, IOrderable
{
    // With our without the following code, this program
    // still compiles.
    //new void Sort ();
}

internal class ImplementingClass : IStackable
{
    public void Sort ()
    {
        Console.WriteLine ("I just sorted your records.");
    }
}

class MethodVersioning
{
    static void Main()
    {
        ImplementingClass ic = new ImplementingClass();
        ic.Sort ();

        Console.Write ("\n\nPress <ENTER> to end: ");
        Console.ReadLine();
    }
}
