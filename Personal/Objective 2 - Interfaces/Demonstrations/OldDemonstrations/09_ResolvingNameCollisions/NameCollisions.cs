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
    // Implementation for IFileWriter.SaveData().
    void IFileWriter.SaveData()
    {
        Console.WriteLine ("Calling IFileWriter.SaveData() to write data.");
    }

    // Implementation for IDbWriter.SaveData().
    void IDbWriter.SaveData()
    {
        Console.WriteLine ("Calling IDbWriter.SaveData() to write data.");
    }
}

class NameCollisionsTest
{
    static void Main()
    {
        // Create an object instance of NameCollisions.
        NameCollisions test = new NameCollisions();

        // Call the appropriate method by casting
        // the object to the appropriate interface.
        ((IFileWriter)test).SaveData();
        ((IDbWriter)test).SaveData();

        Console.WriteLine();

        // Or we can create interface references and 
        // call the methods with these references. Notice
        // the use of the "as" keyword here. This is a 
        // good practice.
        IFileWriter iFile = test as IFileWriter;
        if (iFile != null)
            iFile.SaveData();

        IDbWriter iDB = test as IDbWriter;
        if (iDB != null)
            iDB.SaveData();

        Console.Write ("\n\nPress <ENTER> to end: ");
        Console.ReadLine();
    }
}
