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
                Console.WriteLine("\nBook Title: {0}\nBook Price: {1}\n",
                    this[i].Title, this[i].Price.ToString("C"));
            }
        }

        internal Book this[int index]
        {
            get { return books[index]; }
            set { books.Add(value); }
        }
    }

    // TODO: Create an interface that defines two read only
    //          properties for Title (string) and Price (decimal).

    


    // TODO: Implement the interface created above in the Book
    //      class below.
    internal class Book 
    {
        private string _title;
        private decimal _price;

        internal Book(string title, decimal price)
        {
            _title = title;
            _price = price;
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
