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
            foreach (Book book in books)
            {
                Console.WriteLine(book.GetBookInfo());
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
    
    // TODO: Implement the IBookMethods interface in the Book
    //      class below.  This is in addition to the BookProperties
    //      interface already implemented.
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

        // TODO: Implement GetBookInfo here. Return a string that
        //      has the following:
        //          Book Title: Title
        //          Book Price: Price (currency format)



        public string GetBookInfo()
        {
            return "Title: " + Title + "; Price: " + Price.ToString("C");
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
