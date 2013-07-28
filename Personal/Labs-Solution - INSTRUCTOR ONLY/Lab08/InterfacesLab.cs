using System;
using System.Collections.Generic;
using System.IO;

namespace InterfacesLab
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
                // TODO: Revise the line of code below to execute
                //      the GetBookInfo method through the interface
                //      reference.  First you will need to declare the
                //      interface type and set its reference to the book.
                IBookMethods ibm = (IBookMethods)this[i];
                Console.WriteLine(ibm.GetBookInfo());
            }
        }

        internal Book this[int index]
        {
            get { return books[index]; }
            set { books.Add(value); }
        }
    }

    internal interface IBookProperties
    {
        string Title { get; }
        decimal Price { get; }
    }

    internal interface IBookMethods
    {
        string GetBookInfo();
    }
    
    [Serializable]
    internal class Book : IBookProperties, IBookMethods
    {
        private string _title;
        private decimal _price;

        internal Book(string title, decimal price)
        {
            _title = title;
            _price = price;
        }

        public string Title
        {
            get { return _title; }
        }

        public decimal Price
        {
            get { return _price; }
        }

        public string GetBookInfo()
        {
            string output = "\nBook Title: " + Title + "\nBook Price: " + 
                Price.ToString("C") + "\n";
            
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

            PrintAverageBookPrice(books[0].Price, books[1].Price, books[2].Price);

            Console.Write("\n\nPress <ENTER> to end: ");
            Console.ReadLine();
        }
    }
}
