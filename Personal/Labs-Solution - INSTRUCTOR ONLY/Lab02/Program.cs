using System;
using System.IO;


namespace DirectoryFileInfo
{
    class Program
    {
        const string _PROJECT_ROOT_DIRECTORY_String = @"..\..\..\Lab02";

        static void Main()
        {
            DirectoryInfo projectDirectory = new DirectoryInfo(_PROJECT_ROOT_DIRECTORY_String);

            // TODO: Declare and create a collection of sub directories directly under
            //      the project directory.
            DirectoryInfo[] directories = projectDirectory.GetDirectories();

            // TODO: Declare and create a collection of files directly under
            //      the project directory.
            FileInfo[] files = projectDirectory.GetFiles();

            // TODO: Display the number of directories in the collection
            //      on the console.
            Console.WriteLine("Number of directories under the {0} directory is : {1}",
                projectDirectory.FullName, directories.GetLength(0).ToString());

            // TODO: Display the number of files in the collection
            //      on the console.
            Console.WriteLine("Number of files under the {0} directory is : {1}",
                projectDirectory.FullName, files.Length.ToString());

            Console.WriteLine("\nPress any key to end.");
            Console.ReadLine();
        }
    }
}
