using System;
using System.Collections.Generic;
using System.Text;

namespace InterfaceDemo
{
    internal class Collection
    {
        private int [] _internalList;

        public Collection(int size)
        {
            _internalList = new int[size];
            GenerateNumbers();
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

            Console.Clear();

            Console.Write("How many numbers do you want to generate: ");
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
                Console.WriteLine("\tE. Exit.");
                Console.Write("\nYour choice: ");

                string userOption = Console.ReadLine();
                userOption = userOption.ToUpper();
                int targetNumber = -1;

                switch (userOption)
                {
                    case "1":
                        targetNumber = GetTargetNumber();
                        Console.WriteLine("NOT IMPLEMENTED!");

                        break;

                    case "2":
                        targetNumber = GetTargetNumber();
                        Console.WriteLine("NOT IMPLEMENTED!");

                        break;

                    case "3":
                        Console.Write("\nThe list: {0}\n",
                            listOfNumbers.ToString());
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
