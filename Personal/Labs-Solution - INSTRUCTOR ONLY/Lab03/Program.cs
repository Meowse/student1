using System;
using System.IO;

namespace FileProcessing
{
    class Program
    {
        const string _FULL_FILE_NAME_String = @"..\debug\KingCountyCities.txt";

        static void Main()
        {
            // TODO: Open the text file in a using block using the _FULL_FILE_NAME_String
            //      constant declared above for the path and file name.
            using (StreamReader sr = File.OpenText(_FULL_FILE_NAME_String))
            {
                // TODO:  Within the using block, loop through the file displaying
                //                  each record on the console.
                while (!sr.EndOfStream)
                {
                    Console.WriteLine(sr.ReadLine());
                }
            }  
                    
                
            

            Console.Write("\nPress any key to end.");
            Console.ReadLine();
        }
    }
}
