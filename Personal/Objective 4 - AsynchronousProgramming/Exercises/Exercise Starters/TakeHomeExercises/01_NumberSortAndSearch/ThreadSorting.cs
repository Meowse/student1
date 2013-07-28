using System;
using System.Threading;

namespace ThreadSorting
{
    // This class contains the Insertion Sort algorithm.
    internal class InsertionSort
    {
        private int[] _numbers;

        // Constructor that takes the list of numbers and 
        // stores it locally.
        internal InsertionSort(int[] NumList)
        {
            _numbers = NumList;
        }

        // Property that sorts the list and returns the 
        // sorted list back to the client.
        internal int[] GetSortedList()
        {
            // Loop to compare numbers for sorting.
            for (int i = 1; i < _numbers.Length; i++)
            {
                int j = i;
                int element = _numbers[i];

                // Swap numbers if they are out of order.
                while ((j > 0) && (_numbers[j - 1] > element))
                {
                    _numbers[j] = _numbers[j - 1];
                    j--;
                }

                _numbers[j] = element;
            }

            return _numbers;
        }
    }

    // This class contains the Heap Sort algorithm.
    internal class HeapSort
    {
        private int[] _numbers;

        // Constructor that takes the list of numbers and 
        // stores it locally.
        internal HeapSort(int[] NumList)
        {
            _numbers = NumList;
        }

        // Property that sorts the list and returns the 
        // sorted list back to the client.
        internal int[] GetSortedList()
        {
            int len = _numbers.Length;

            for (int i=len/2; i > 0; i--)
            {
                downHeap(_numbers, i, len);
            }

            do
            {
                int element = _numbers[0];
                _numbers[0] = _numbers[len - 1];
                _numbers[len - 1] = element;

                len -= 1;

                downHeap(_numbers, 1, len);
            } while (len > 1);

            return _numbers;
        }

        private void downHeap (int[] list, int index, int len)
        {
            int element = list[index-1];

            while (index <= len/2)
            {
                int j = index + index;

                if ((j < len) && (list[j-1] < list[j]))
                {
                    j++;
                }

                if (element >= list[j-1])
                {
                    break;
                }
                else
                {
                    list[index-1] = list[j-1];
                    index=j;
                }
            }

            list[index-1] = element;
        }
    }

    // This class contains the searching algorithm 
    // to locate the number of occurances of a 
    // specific number in the list. This algorithm 
    // is based on the binary search.
    internal class NumberLocator
    {
        private int[] _numbers;
        private int element;
        private int occurances;

        // Constructor takes the ordered list and the 
        // number to search for.
        internal NumberLocator(int[] List, int Element)
        {
            _numbers = List;
            element = Element;
        }

        // Using a binary search, locate the first 
        // occurrance of the number.
        private void FindOccurances ()
        {
            int high = _numbers.Length;
            int i;

            for (int low=-1; high-low>1; )
            {
                i = (high+low) / 2;

                if (element <= _numbers[i])
                    high = i;
                else
                    low = i;
            }

            // If we found one occurance, see if there 
            // are any duplicates.
            if (element == _numbers[high])
            {
                for (int j=high; j < _numbers.Length; j++)
                {
                    if (_numbers[j] == element)
                        occurances += 1;
                    else
                        break;
                }

                for (int j=(high-1); j >= 0; j--)
                {
                    if (_numbers[j] == element)
                        occurances += 1;
                    else
                        break;
                }
            }
        }

        // Property used to return the number of occurances 
        // of a specific number in the ordered list.
        internal int Frequency
        {
            get 
            {
                FindOccurances();
                return occurances;
            }
        }
    }

    // Class used to generate a list of random numbers.
    internal class NumberGenerator
    {
        private int listSize = 0;
        private int[] numList;

        // Constructor that takes the size of the list that 
        // it will generate.
        internal NumberGenerator(int ListSize)
        {
            listSize = ListSize;
            numList = new int[listSize];
        }

        private void GenerateNumbers()
        {
            // Using the Random class, generate random 
            // numbers based on the current time of the 
            // computer. This will hopefully create a 
            // very unique list.
            Random rnd = new Random((int)DateTime.Now.Ticks);

            for (int i=0; i < numList.Length; i++)
            {
                numList[i] = rnd.Next (1, 1000);

                // Sleep for a moment to make sure that the 
                // system clock advances enough to generate a 
                // unique list.
                Thread.Sleep(25);
            }
        }

        // Property to return a unique, unordered list.
        internal int[] List
        {
            get
            {
                GenerateNumbers();
                return numList;
            }
        }
    }

    // Class that is used to sort a list and search the 
    // ordered list for the number of times a specific number 
    // occurs.
    internal class SortAndSearch
    {
        // Mutex used to lock the sorting algorithm.
        // TODO: Declare a mutex for the sorting algorithm.

        // Mutex used to lock the printing and searching algorithms
        // TODO: Declare a mutex for the printing and seaching algorithms.

        // Enumeration to indicate the type of 
        // search to perform.
        internal enum SortType
        {
            InsertionSort,
            HeapSort
        }

        private int _size = 0;
        private SortType _sortMethod;
        private int[] _newList;
        private int _num;

        // Constructor that takes the list size, the number to search
        // for, and the type of sort to perform.
        internal SortAndSearch(int ListSize, int Number, SortType sortDesired)
        {
            _size = ListSize;
            _num = Number;
            _sortMethod = sortDesired;
        }

        // Prints the ordered list to the console.
        private void PrintList (int[] list)
        {
            Console.Write ("{0,3}", list[0]);

            for (int i=1; i < list.Length; i++)
            {
                Console.Write (", {0,3}", list[i]);
            }

            Console.WriteLine ("\n\n\n");
        }

        // The method that does it all!
        internal void SortingMethod()
        {
            // Generate the unordered list.
            NumberGenerator NG = new NumberGenerator(_size);

            // Depending on the sort to use, create an 
            // object instance of the appropriate sort class.
            if (_sortMethod == SortType.InsertionSort)
            {
                InsertionSort IS;

                // TODO: Lock the sorting mutex

                // Sort and retrieve the list.
                int[] list = NG.List;
                IS = new InsertionSort(list);
                _newList = IS.GetSortedList();

                // TODO: Unlock the sorting mutex.
            }
            else
            {
                HeapSort HS;

                // TODO: Lock the sorting mutex.

                // Sort and retrieve the list.
                int[] list = NG.List;
                HS = new HeapSort(list);
                _newList = HS.GetSortedList();
 
                // TODO: Unlock the sorting mutex.
            }
 
            // TODO: Lock the print and search mutex.

            // Print the list to the console.
            PrintList(_newList);

            // Search for a specific number and its 
            // occurances in the list.
            SearchingMethod();

            // TODO: Unlock the print and search mutex.
        }

        private void SearchingMethod ()
        {
            string sortString;

            try
            {
                // Set a string to indicate which method 
                // or sorting was used.
                if (_sortMethod == SortType.InsertionSort)
                    sortString = "Insertion Sort";
                else
                    sortString = "Heap Sort";

                // Attempt to locate the number and display 
                // the results.
                NumberLocator NL = new NumberLocator (_newList, _num);
                Console.WriteLine ("Number {0} occured {1} time(s) in the {2} list.\n",
                    _num,
                    NL.Frequency,
                    sortString);
            }
            catch (Exception e)
            {
                Console.WriteLine ("Exception [ {0} ] occurred in the {1}.\n",
                    e.Message,
                    Thread.CurrentThread.Name);
            }
        }
    }

    // Class that is used in a thread.
    internal class ThreadClass
    {
        private static int _listSize = 0;
        private static SortAndSearch.SortType _sortType;
        private static int _number = 0;

        // Constructor that takes the size of the list, the number to
        // search for, and the type of sorting to perform.
        internal ThreadClass(int ListSize, int Number, SortAndSearch.SortType SortType)
        {
            _listSize = ListSize;
            _number = Number;
            _sortType = SortType;
        }

        // Method that will be called when the thread is started.
        internal static void ThreadMethod()
        {
            SortAndSearch driver = new SortAndSearch (_listSize, _number, _sortType);
            driver.SortingMethod();
        }
    }

    // Class that tests the threading of this program.
    class TestClass
    {
        static void Main()
        {
            int size = 0;
            int num = 0;

            try
            {
                // Prompt for the list size.
                Console.Write ("\n\nHow many numbers would you like to sort (0 to end): ");
                size = int.Parse (Console.ReadLine());

                // If the list size is > 0, proceed. Otherwise, end.
                if (size > 0)
                {
                    // Prompt for a number to search for in the ordered lists.
                    Console.Write ("Enter a number between 1 and 1000 to search for: ");
                    num = int.Parse(Console.ReadLine());

                    Console.WriteLine ("Generating sorted numbers list...\n");

                    // Create the first thread object - this will run the insertion sort.
                    // TODO: Create an instance of the ThreadClass.
                    // TODO: Create a ThreadStart delegate for ThreadClass.
                    // TODO: Create the thread object and give it the delegate.
                    // TODO: Set the name property of the thread.
                    
                    // Create the second thread object - this will run the heap sort.
                    // TODO: Create an instance of the ThreadClass.
                    // TODO: Create a ThreadStart delegate for ThreadClass.
                    // TODO: Create the thread object and give it the delegate.
                    // TODO: Set the name property of the thread.

                    // Start the threads.
                    // TODO: Start the threads.

                    // Join the threads so the application 
                    // doesn't end prematurely.
                    // TODO: Synch up with the threads so the app doesn't end early.
                }
            }
            catch (Exception e)
            {
                Console.WriteLine (e.Message);
            }
            finally
            {
                Console.Write ("\n\n\nPress <ENTER> to end: ");
                Console.ReadLine ();
            }
        }
    }
}
