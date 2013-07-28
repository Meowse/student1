/************************************************************************
 * 
 * LocateSpecificFiles: Part 3
 * 
 * Core Topics:
 * 1. StreamReader class methods.
 * 2. Get files matching a search criteria.
 * 3. Using block for opening, processing and closing a file.
 * 4. OpenText method of File class.
 **********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LocateSpecificFiles
{
    class Program
    {
        const string _ROOT_DIRECTORY_NAME_String = @"..\..\";
        
        static void Main()
        {
            FileInfo[] files;
            DirectoryInfo rootDirectory;
            StreamReader sr;

            try
            {
                try
                {
                    rootDirectory = 
                        new DirectoryInfo(_ROOT_DIRECTORY_NAME_String);
                }
                catch
                {
                    Console.WriteLine("{0} directory not found.", 
                        _ROOT_DIRECTORY_NAME_String);
                    return;
                }

                files = 
                    rootDirectory.GetFiles("*.txt", SearchOption.AllDirectories);

                if (files.GetLength(0) == 0)
                {
                    Console.WriteLine("There are no text files in {0} directory.", 
                        _ROOT_DIRECTORY_NAME_String);
                    return;
                }

                foreach (FileInfo file in files)
                {
                    Console.WriteLine("The following are contents of file {0}:\n", 
                        file.Name);
                    
                    using (sr = File.OpenText(file.FullName))
                    {
                        while (sr.Peek() != -1)
                        {
                            Console.WriteLine("\t" + sr.ReadLine());
                        }
                        Console.WriteLine();
                    }
                }
            }
            finally
            {
                Console.Write("\nPress any key to end.");
                Console.ReadLine();
            }
        }
    }
}