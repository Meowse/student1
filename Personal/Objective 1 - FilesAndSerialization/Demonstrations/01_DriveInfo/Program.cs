/************************************************************************
 *
 * DriveInfo: Part 1
 * 
 * Core Topics:
 * 1. Using directive for System.IO.
 * 2. DriveInfo class properties and methods.
 * 3. DriveInfo collection.
 **********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DriveInfoProgram
{
    class Program
    {
        const decimal _KILO_BYTES_Decimal = 1024.0m;
        
        static void Main()
        {
            decimal driveFreeSpaceDecimal;
            
            DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            Console.WriteLine("Computer Drives Available:\n");
            Console.WriteLine("\n\tDrive\tDrive Type\tSpace Available");
            Console.WriteLine("\t_____\t__________\t_______________\n");
            foreach (DriveInfo di in drives)
            {
                if (di.IsReady)
                {
                    driveFreeSpaceDecimal = 
                        di.AvailableFreeSpace / _KILO_BYTES_Decimal /
                            _KILO_BYTES_Decimal;
                    Console.WriteLine("\n\t{0}\t{1}\t\t{2}", 
                        di.Name, 
                        di.DriveType.ToString(), 
                        driveFreeSpaceDecimal.ToString("N0") + " MB");
                }
            }

            Console.Write("\nPress any key to end.");
            Console.ReadLine();
        }
    }
}
