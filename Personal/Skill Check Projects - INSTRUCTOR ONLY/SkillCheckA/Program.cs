/* Project Name:    SkillCheckA
 * Developer:       Carol C. Torkko, MCSD - Bellevue College
 * Date:            July, 2011
 * Description:     Acquire info about directories and files.
 */

using System;
// TODO: Specify a using directive where most directory and file
//      types are located.


public class Program
{        
    // Constants below will be used to set a reference to the main directory
    //      of this skill check.
    const string _ROOT_DIRECTORY_NAME_String = @"..\..\..\";
    const string _DIRECTORY_NAME_String = "SkillCheckA";
        
    // Retrieve all directories and files and display information about them.
    static void DisplayDirectoryInfo(DirectoryInfo[] directories, 
        DirectoryInfo rootDirectory, int levelInt)
    {
        FileInfo[] files;

        for (int i = 1; i <= levelInt; i++)
        {
            Console.Write("   ");
        }

        // TODO: Load the pre-declared "files" collection with references to all 
        //      files directly in the current directory ("rootDirectory").
        

        // TODO: Complete the WriteLine method below to specify the following
        //      info about the current directory ("rootDirectory"):
        //          0.  Its name.
        //          1.  Number of files.
        //          2.  Number of sub directories.
        Console.WriteLine("{0}: Files = {1}; Sub Directories = {2}", 
            );

        ++levelInt;

        if (directories.GetLength(0) > 0)
        {
            foreach (DirectoryInfo di in directories)
            {
                // TODO: Complete this method by calling it recursively
                //      for each directory ("di") in the "directories" collection.  
                
            }
        }

        // Loop through each file in the current directory to display its info.
        foreach (FileInfo file in files)
        {
            for (int i = 1; i <= levelInt; i++)
            {
                Console.Write("   ");
            }

            // TODO: Complete the WriteLine method below to specify 3
            //      properties about the "file":
            //          0.  Its name.
            //          1.  The last time it was written.
            //          2.  Its size.
            Console.WriteLine("{0}: {1}; {2} bytes", 
                );
        }

    }

    static void Main()
    {
        DirectoryInfo[] directories;
        FileInfo[] files;
        DirectoryInfo rootDirectory;
        DirectoryInfo projectDirectory;
        StreamReader sr;

        try
        {
            // Set reference to the Skill Check Projects folder.
            rootDirectory =
                new DirectoryInfo(_ROOT_DIRECTORY_NAME_String);

            // Set reference to the SkillCheckA folder.
            directories =
                rootDirectory.GetDirectories(_DIRECTORY_NAME_String,
                    SearchOption.AllDirectories);

            if (directories.GetLength(0) == 0)
            {
                Console.WriteLine("{0} directory not found.",
                    _DIRECTORY_NAME_String);
                return;
            }
            else
            {
                Console.WriteLine(
                    "\nFollowing is a list of sub directories and files for {0} directory: \n",
                    _DIRECTORY_NAME_String);

                // Call method to display info about files and sub directories within this
                //      project's directory.
                foreach (DirectoryInfo di in directories)
                {
                    DisplayDirectoryInfo(di.GetDirectories(), di, 1);
                }
            }

            // Set a reference to the main folder of this project
            projectDirectory = directories[0];

            // TODO: Store all .txt file references in the pre-declared name "files"
            //      (collection of FileInfo type).  The .txt files are located within
            //      different sub directories of the root directory ("rootDirectory", a DirectoryInfo
            //      type).
            

            if (files.GetLength(0) == 0)
            {
                Console.WriteLine("There are no text files in {0} directory.",
                    _ROOT_DIRECTORY_NAME_String);
                return;
            }

            // Loop through each text reference in the collection to open and
            //      display their contents.
            foreach (FileInfo file in files)
            {
                Console.WriteLine("\nThe following are contents of file {0}:\n",
                    file.Name);

                // TODO: Process each text file as follows:
                //  1.  Open, process and close the text file in a "using" block.
                //  2.  Use the pre-declared name sr (StreamReader type) as the
                //          reference to the file.
                //  3.  Read each record in the file and display its contents on the Console.

            
            }
        }
        finally
        {
            Console.Write("\nPress any key to end.");
            Console.ReadLine();
        }
    }
}
