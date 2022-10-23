using BookCollection.Models;
using Booked.Controllers;

namespace Booked.SupportClasses
{
    public class BookValidator
    {
        /// <summary>
        /// Collects problems with the book and adds it to a string. Returns empty string if no problems found.
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public static string PossibleBookProblems(Book book)
        {
            var problems = String.Empty;

            if (String.IsNullOrEmpty(book.Title))
                problems += "Title field is empty. ";

            if (String.IsNullOrEmpty(book.Author))
                problems += "Author field is empty. ";

            if (String.IsNullOrEmpty(book.Publisher))
                problems += "Publisher field is empty. ";

            var books = SQLController.GetAllDBBooks();

            var identicalBooksFound = books.Where(i => i.Title == book.Title && i.Year == book.Year && i.Author == book.Author).Any();

            if (identicalBooksFound)
                problems += "Book with same title, author and year found already.";

            return problems;
        }

        /// <summary>
        /// Collects problems with book query filters to a string. Return empty string if no problems found.
        /// </summary>
        /// <param name="author"></param>
        /// <param name="year"></param>
        /// <param name="publisher"></param>
        /// <returns></returns>
        public static string PossibleQueryProblems(string? author, int? year, string? publisher)
        {
            var problems = String.Empty;

            if (author != null)
            {
                if (author == "")
                    problems += "Author field is empty. ";
            }

            if (publisher != null)
            {
                if (publisher == "")
                    problems += "Publisher field is empty. ";
            }

            if (year != null)
            {
                //int i = 0;
                //var correctYear = int.TryParse(year, out i);

                //if (correctYear == false)
                //    problems += "Incorrect year. ";
            }

            return problems;
        }
    }
}
