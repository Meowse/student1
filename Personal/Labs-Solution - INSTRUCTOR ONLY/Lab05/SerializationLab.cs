using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SerializationLab
{
    internal class Books
    {
        private List<Book> books;

        internal Books(int bookTotalInt)
        {
            books = new List<Book>(bookTotalInt);
        }

        internal void PrintBooks()
        {
            for (int i = 0; i < books.Count; i++)
            {
                Console.WriteLine(this[i].ToString());
            }
        }

        internal Book this[int index]
        {
            get { return books[index]; }
            set { books.Add(value); }
        }
    }
    
    [Serializable]
    internal class Book
    {
        private string _title;
        private decimal _price;

        internal Book(string title, decimal price)
        {
            _title = title;
            _price = price;
        }

        public override string ToString()
        {
            string output = "\nBook Title: " + _title +
                "\nBook Price: " + _price.ToString("C") + "\n";
            return output;
        }
    }
    
    internal class CollectionTester
    {
        static void PrintAverageBookPrice(params decimal[] prices)
        {
            decimal bookTotalPricesDecimal = 0;

            for (int i = 0; i <= prices.GetUpperBound(0); i++)
            {
                bookTotalPricesDecimal += prices[i];
            }

            decimal averagePriceDecimal = bookTotalPricesDecimal / prices.GetLength(0);
            Console.WriteLine("Average price of books is: {0} \n", averagePriceDecimal.ToString("C"));
        }

        static void Main()
        {
            Books books = new Books(3);
            
            Book book = new Book("The Master Plan", 34.17m);
            books[0] = book;

            book = new Book("The Avenger", 45.23m);
            books[1] = book;

            book = new Book("Granite Falls", 14.78m);
            books[2] = book;

            books.PrintBooks();

            PrintAverageBookPrice(34.17m, 45.23m, 14.78m);

            // Create file to use in serializing.
            string bookFullFileName = @"..\debug\GraniteFalls.dat";

            // Open a file and serialize the Granite Falls book.
            using (FileStream bookFile = new FileStream(bookFullFileName, FileMode.Create,
                FileAccess.Write, FileShare.None))
            {
                BinaryFormatter binFormat = new BinaryFormatter();
                binFormat.Serialize(bookFile, book);
            }

            Console.WriteLine("\nThe following book has been serialized:\n{0}", book.ToString());
            book = null;

            // TODO: Open a FileStream type file for reading in a using block.  Name of file is contained 
            //              in the variable bookFullFileName.  Deserialize the contents of the file and set
            //              the reference in the variable named book (declared above).
            using (FileStream bookFile = File.OpenRead(bookFullFileName))
            {
                BinaryFormatter binFormat = new BinaryFormatter();
                book = (Book)binFormat.Deserialize(bookFile);
            }


            Console.WriteLine("\nThe following book has been deserialized:\n{0}", book.ToString());

            Console.Write("\n\nPress <ENTER> to end: ");
            Console.ReadLine();
        }
    }
}
