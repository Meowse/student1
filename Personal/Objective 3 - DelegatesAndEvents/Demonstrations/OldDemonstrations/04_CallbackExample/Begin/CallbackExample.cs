/**************************************************************************
 * 
 *  This program shows how to create a callback method and use a 
 *  delegate to call it. This program is still synchronous, meaning
 *  that when the reading process starts, the Main() method is hung
 *  until reading completes. Later we will look at asynchronous 
 *  programming that will allow the Main() method to continue to
 *  run while the reading process is running as well.
 * 
 **************************************************************************/

using System;

namespace CallbackExample
{
    // TODO: Create an internal delegate named ReadDone that takes no
    //       arguments and returns void.

    // This class simulates reading data from a data source.
    internal class DataSourceReader
    {
        // TODO: Modify this method signature to accept a single argument
        //       of type ReadDone. This is the delegate that will be passed
        //       in when this method is called.
        internal void ReadFromDataSource ()
        {
            // Use a random number generator to determine a delay time
            // between each "read". This helps to simulate a long-running
            // process.
            Random r1 = new Random(DateTime.Now.Millisecond);
            int waitTime = r1.Next (0, 500);

            // Use a random number generator to determine how many data
            // records need to be read.
            Random r2 = new Random(DateTime.Now.Second);
            int numberOfRecordsToRead = r2.Next (10, 25);

            Console.WriteLine ("\n\t" +
                "Reading {0} records with a wait time of {1} milliseconds.",
                numberOfRecordsToRead, waitTime);

            // Create a simulated read process.
            for (int i=0; i < numberOfRecordsToRead; i++)
            {
                // Pause to simulate a long-running process.
                System.Threading.Thread.Sleep (waitTime);

                // Display an update to the console every so often.
                if (i%5 == 0)
                {
                    Console.WriteLine ("\tRead {0} records so far.", i);
                }
            }

            // TODO: Using the delegate that was passed into this method, 
            //       call the Callback method here.
        }
    }

    class CallbackExampleDriver
    {
        // TODO: Write a method named ReaderCallback that takes no
        //       arguments and returns void. This method will just display
        //       status to the console window that reading is done.

        static void Main()
        {
            // TODO: Create an instance of the CallbackExampleDriver class.

            DataSourceReader reader = new DataSourceReader();

            // TODO: Create an instance of the ReadDone delegate providing 
            //       it the ReaderCallback method found in this class.

            Console.WriteLine ("Starting to read in the data...");

            // TODO: Modify this call to include one argument which is the
            //       delegate object created above.
            reader.ReadFromDataSource();

            Console.Write ("\nPress <ENTER> to end: ");
            Console.ReadLine();
        }
    }
}
