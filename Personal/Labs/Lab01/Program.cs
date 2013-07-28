using System;
//TODO: Add the correct using directive for the DriveInfo class.


namespace DriveInfoLab
{
    class Program
    {
        static void Main()
        {
            // TODO: Declare and create a collection for the drives on the computer.
            //              Use the name, drives, for the reference pointer to the collection.
            

            foreach (DriveInfo drive in drives)
            {
                // TODO: Display the drive info below ONLY if the drive is ready.

                Console.WriteLine("Drive: {0}; Drive Type: {1}", drive.Name, drive.DriveType);

            }

            Console.WriteLine("\nPress any key to end.");
            Console.ReadLine();
        }
    }
}
