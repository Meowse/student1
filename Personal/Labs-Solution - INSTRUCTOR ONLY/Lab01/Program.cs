using System;
//TODO: Add the correct using directive for the DriveInfo class.
using System.IO;

namespace DriveInfoLab
{
    class Program
    {
        static void Main()
        {
            // TODO: Declare and create a collection for the drives on the computer.
            //              Use the name, drives, for the reference pointer to the collection.
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                // TODO: Display the drive info below ONLY if the drive is ready.
                if (drive.IsReady)
                {
                    Console.WriteLine("Drive: {0}; Drive Type: {1}", drive.Name, drive.DriveType);
                }
            }

            Console.WriteLine("\nPress any key to end.");
            Console.ReadLine();
        }
    }
}
