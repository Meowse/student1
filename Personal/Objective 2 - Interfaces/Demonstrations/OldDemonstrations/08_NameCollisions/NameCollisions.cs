// File: NameCollisions.cs
using System;

// Interface that writes data to a file.
internal interface IFileWriter
{
    void SaveData();
}

// Interface that writes data to a database.
internal interface IDbWriter
{
    void SaveData();
}

// Class that implements both interfaces.
internal class NameCollisions : IFileWriter, IDbWriter
{
    // Which interface has been implemented here? We don't really know. In 
    // addition, this violates interface implementation. This class should 
    // implement two methods called SaveData, one for each interface being
    // implemented.
    public void SaveData()
    {
        Console.WriteLine ("Saving data in class NameCollisions.");
    }
}

class NameCollisionsTest
{
    static void Main()
    {
        // Create an object instance of NameCollisions and 
        // call the SaveData() method.
        NameCollisions test = new NameCollisions();

        if (test is IFileWriter)
            test.SaveData();

        if (test is IDbWriter)
            test.SaveData();

        Console.Write ("\n\nPress <ENTER> to end: ");
        Console.ReadLine();
    }
}
