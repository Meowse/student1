using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CovarianceAndContravarianceLab
{
    class Book
    { }

    class StoryBook : Book
    { }

    class ComedyBook : Book
    { }

    class Program
    {
        // Covariance means only one delegate is needed to cover
        //  all books instead of a separate delegate for
        //  each type of book on the return type.
        delegate Book NewBookDelegate();

        // Contravariance means a need to declare a separate
        //  delegate for each parameter type.  However, only
        //  one method is needed with a parameter type that is
        //  a base class to the parameter types in each of the
        //  delegates.
        delegate void ShowStoryBookDelegate(StoryBook s);
        delegate void ShowComedyBookDelegate(ComedyBook c);

        static StoryBook NewStoryBook()
        {
            return new StoryBook();
        }

        static ComedyBook NewComedyBook()
        {
            return new ComedyBook();
        }

        // Method called only once, but executed multiple times
        //      for each delegate in the delegate collection.
        static void AddBook(NewBookDelegate newBooks)
        {
            // Loop is needed to capture the return value from each delegate
            //      call.
            foreach (NewBookDelegate nbd in newBooks.GetInvocationList())
            {
                books.Add(nbd());
            }
        }

        // Contravariance means only one method is needed to show all
        //      types of books.
        static void ShowBook(Book b)
        {
            Console.WriteLine(b.ToString());
        }

        static List<Book> books;

        static void Main()
        {
            books = new List<Book>();

            // Covariance being applied by adding methods to a delegate with
            //  different return types.
            NewBookDelegate book = NewStoryBook;
            // TODO: Add a method that creates a new instance of the ComedyBook
            //      to the NewBookDelegate referenced by the name of "book".
            book += NewComedyBook;

            // Covariance adds efficiency by needing to call method below
            //  only once instead of multiple times for each book.
            AddBook(book);

            // Contravariance means only one method needed to be coded to
            //  be referenced by any delegate with a parameter type that is a 
            //  derivation of the parameter in the method being referenced.
            ShowStoryBookDelegate showStoryBook = ShowBook;
            showStoryBook(NewStoryBook());

            // TODO:  Declare the ShowComedyBookDelegate and set the
            //      ShowBook method into it.
            ShowComedyBookDelegate showComedyBook = ShowBook;

            // TODO:  Call the ShowComedyBookDelegate delegate using the
            //      reference name you assigned in the declaration above.  The 
            //      value passed in is the return value from the NewComedyBook
            //      method.
            showComedyBook(NewComedyBook());

            Console.WriteLine("\nThe type of books created are (proving covariance): ");
            // Loop through the books collection and display the type of each
            //      book on the Console window.
            foreach (Book b in books)
            {
                Console.WriteLine("\t{0}", b.ToString());
            }
            
            Console.Write("Press any key to end.");
            Console.ReadLine();
        }
    }
}