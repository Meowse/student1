/************************************************************************
 * 
 * Interfaces: Part 3
 * 
 * Core Topics:
 * 1. Apply polymorphism to sort two difference class types that 
 *          implement the same interface.
 **********************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace InterfaceDemo
{
    interface ISortable
    {
        void Sort();
    }

    interface ISearchable
    {
        int IndexOf(object o);
        bool Found(object o);
    }

    internal class Collection : ISearchable, ISortable
    {
        private int [] _internalList;

        public Collection(int size)
        {
            _internalList = new int[size];
            GenerateNumbers();
        }

        public int IndexOf(object o)
        {
            int index = -1;
            int targetData = 0;

            try
            {
                targetData = (int)o;

                for (int i = 0; i < _internalList.Length; i++)
                {
                    if (_internalList[i] == targetData)
                    {
                        index = i;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                index = -1;
                Console.WriteLine("\nEXCEPTION: {0}", e.Message);
                Console.WriteLine("           {1}", e.StackTrace);
            }

            return index;
        }

        public bool Found(object o)
        {
            bool found = false;
            int index = 0;
            int targetData = 0;

            try
            {
                targetData = (int)o;

                while (!found && index < _internalList.Length)
                {
                    if (_internalList[index] == targetData)
                    {
                        found = true;
                    }
                    else
                    {
                        index++;
                    }
                }
            }
            catch (Exception e)
            {
                index = -1;
                Console.WriteLine("\nEXCEPTION: {0}", e.Message);
                Console.WriteLine("           {1}", e.StackTrace);
            }

            return found;
        }

        public void Sort ()
        {
            // Because _internalList is an array, the Array type has a
            // sort method, and the Array type knows how to sort ints,
            // we can simply rely on the Array.Sort() method to do the
            // sorting for us. If we were using our own custom type 
            // rather than int, we would have to implement IComparable
            // to implement how to compare two objects.
            Array.Sort(_internalList);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _internalList.Length; i++)
            {
                sb.AppendFormat("{0}  ", _internalList[i]);
            }

            return sb.ToString();
        }

        private void GenerateNumbers()
        {
            Random r = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < _internalList.Length; i++)
            {
                _internalList[i] = r.Next(1, 10000);
                System.Threading.Thread.Sleep(71);
            }
        }
    }

    class Program
    {
        private static void SortTheList(ISortable unsortedList)
        {
            unsortedList.Sort();
        }

        private static int GetTargetNumber()
        {
            int targetNumber = -1;
            Console.Write("\nWhat number are you looking for: ");
            string userInput = Console.ReadLine();

            if (!int.TryParse(userInput, out targetNumber))
            {
                targetNumber = -1;
            }

            return targetNumber;
        }

        static void Main(string[] args)
        {
            bool keepGoing = true;
            int listSize = 0;
            Collection listOfNumbers = null;
            NewCollection listOfStrings = null;

            Console.Clear();

            Console.Write("How many elements do you want to generate: ");
            string userInput = Console.ReadLine();
            if (!int.TryParse(userInput, out listSize))
            {
                listSize = 0;
            }

            if (listSize <= 0)
            {
                keepGoing = false;
            }
            else
            {
                listOfNumbers = new Collection(listSize);
            }

            while (keepGoing)
            {
                Console.WriteLine("\nChoose from the following options:\n");
                Console.WriteLine
                    ("\t1. Get the index of an item in the list.");
                Console.WriteLine("\t2. See if an item is in the list.");
                Console.WriteLine("\t3. Display the entire list.");
                Console.WriteLine("\t4. Sort the entire list.");
                Console.WriteLine("\t5. Generate a list of strings.");
                Console.WriteLine("\t6. Display the list of strings.");
                Console.WriteLine("\t7. Sort the list of strings.");
                Console.WriteLine("\tE. Exit.");
                Console.Write("\nYour choice: ");

                string userOption = Console.ReadLine();
                userOption = userOption.ToUpper();
                int targetNumber = -1;

                switch (userOption)
                {
                    case "1":
                        targetNumber = GetTargetNumber();
                        if (targetNumber > 0)
                        {
                            int index = listOfNumbers.IndexOf(targetNumber);
                            if (index >= 0)
                            {
                                Console.WriteLine
                                    ("\n{0} is found at index {1}.\n", 
                                    targetNumber, index);
                            }
                            else
                            {
                                Console.WriteLine
                                    ("\n{0} is not in the list.\n", 
                                    targetNumber);
                            }
                        }

                        break;

                    case "2":
                        targetNumber = GetTargetNumber();
                        if (targetNumber > 0)
                        {
                            if (listOfNumbers.Found(targetNumber))
                            {
                                Console.WriteLine("\n{0} is in the list.\n", 
                                    targetNumber);
                            }
                            else
                            {
                                Console.WriteLine
                                    ("\n{0} is NOT in the list.\n", 
                                    targetNumber);
                            }
                        }

                        break;

                    case "3":
                        Console.Write("\nThe list: {0}\n",
                            listOfNumbers.ToString());
                        break;

                    case "4":
                        //ISortable unsortedNumberList = 
                        //    (ISortable)listOfNumbers;
                        //// By doing the following, the only methods that are
                        //// accessible via unsortedNumberList are those that
                        //// are found in ISortable and System.Object.
                        //ISortable unsortedNumberList =
                        //    listOfNumbers as ISortable;

                        //if (null != unsortedNumberList)
                        //{
                        //    SortTheList(unsortedNumberList);
                        
                            SortTheList(listOfNumbers);
                            Console.WriteLine
                                ("\nThe list of numbers has been sorted.\n");

                        //}
                        //else
                        //{
                        //    Console.WriteLine("The current collection " +
                        //        "does not support ISortable!");
                        //}

                        break;

                    case "5":
                        listOfStrings = new NewCollection(listSize);
                        break;

                    case "6":
                        Console.Write("\nThe list: \n{0}",
                            listOfStrings.ToString());
                        break;

                    case "7":
                        //ISortable unsortedStringList =
                        //    (ISortable)listOfStrings;
                        //// By doing the following, the only methods that are
                        //// accessible via unsortedStringList are those that
                        //// are found in ISortable and System.Object.
                        //ISortable unsortedStringList = 
                        //    listOfStrings as ISortable;

                        //if (null != unsortedStringList)
                        //{
                        //    SortTheList(unsortedStringList);
                            SortTheList(listOfStrings);
                            Console.WriteLine
                                ("\nThe list of strings has been sorted.\n");
                        //}
                        //else
                        //{
                        //    Console.WriteLine("The current collection " +
                        //        "does not support ISortable!");
                        //}

                        break;

                    case "E":
                        keepGoing = false;
                        break;

                    default:
                        Console.WriteLine
                            ("\nYour option {0} is not valid. Try again!\n",
                            userOption);
                        break;
                }
            }

            Console.Write("\n\nPress <ENTER> to end: ");
            Console.ReadLine();
        }
    }
}
