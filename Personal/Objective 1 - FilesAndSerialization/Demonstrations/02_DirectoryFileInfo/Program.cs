/************************************************************************
 *
 * DirectoryFileInfo: Part 2
 * 
 * Core Topics:
 * 1. DirectoryInfo class properties and methods.
 * 2. DirectoryInfo collection.
 * 3. FileInfo class properties and methods.
 * 4. FileInfo collection.
 * 5. Recursive method for drilling down at all levels in directories.
 **********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DirectoryFileInfo
{
    class Program
    {
        const string _ROOT_DIRECTORY_NAME_String = @"..\..\..\";
        const string _DIRECTORY_NAME_String = "02_DirectoryFileInfo";
        
        static void DisplayDirectoryInfo(DirectoryInfo[] directories, 
            DirectoryInfo rootDirectory, int levelInt)
        {
            FileInfo[] files;

            for (int i = 1; i <= levelInt; i++)
            {
                Console.Write("   ");
            }

            files = rootDirectory.GetFiles();
            Console.WriteLine("{0}: Files = {1}; Sub Directories = {2}", 
                rootDirectory.Name, 
                files.GetLength(0).ToString(), 
                directories.GetLength(0).ToString());

            ++levelInt;

            if (directories.GetLength(0) > 0)
            {
                foreach (DirectoryInfo di in directories)
                {
                    DisplayDirectoryInfo(di.GetDirectories(), di, levelInt);
                }
            }

            foreach (FileInfo file in files)
            {
                for (int i = 1; i <= levelInt; i++)
                {
                    Console.Write("   ");
                }

                Console.WriteLine("{0}: {1}; {2} bytes", 
                    file.Name, file.LastWriteTime, file.Length);
            }

        }

        static void Main()
        {
            DirectoryInfo[] directories;

            DirectoryInfo rootDirectory = 
                new DirectoryInfo(_ROOT_DIRECTORY_NAME_String);
            directories = 
                rootDirectory.GetDirectories(_DIRECTORY_NAME_String, 
                    SearchOption.AllDirectories);

            if (directories.GetLength(0) == 0)
            {
                Console.WriteLine("{0} directory not found.", 
                    _DIRECTORY_NAME_String);
            }
            else
            {
                Console.WriteLine(
                    "\nFollowing is a list of sub directories and files for {0} directory: \n", 
                    _DIRECTORY_NAME_String);

                foreach (DirectoryInfo di in directories)
                {
                    DisplayDirectoryInfo(di.GetDirectories(), di, 1);
                }
            }

            Console.Write("\nPress any key to end.");
            Console.ReadLine();
        }
    }
}
