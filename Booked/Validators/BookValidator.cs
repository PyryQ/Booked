using Booked.Controllers;
using Booked.Models.Interfaces;

namespace Booked.SupportClasses
{
    public class BookValidator
    {
        /// <summary>
        /// Collects problems with the book and adds it to a string. Returns list of possible problems.
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public static string PossibleBookProblems(IBook book)
        {
            var problems = new List<string>();

            if (String.IsNullOrEmpty(book.Title))
                problems.Add("Title field is empty.");

            if (String.IsNullOrEmpty(book.Author))
                problems.Add("Author field is empty.");

            if (String.IsNullOrEmpty(book.Publisher))
                problems.Add("Publisher field is empty.");

            var books = SQLController.GetAllDBBooks();

            var identicalBooksFound = books.Where(i => i.Title == book.Title && i.Year == book.Year && i.Author == book.Author).Any();

            if (identicalBooksFound)
                problems.Add("Book with same title, author and year found already.");

            if (problems.Any())
                return String.Join(" ", problems.ToArray());
            else
                return String.Empty;
        }

        /// <summary>
        /// Collects problems with book query filters to a string. Return empty string if no problems found.
        /// </summary>
        /// <param name="author"></param>
        /// <param name="year"></param>
        /// <param name="publisher"></param>
        /// <returns></returns>
        public static string PossibleQueryProblems(string? author, decimal? year, string? publisher)
        {
            var problems = new List<string>();

            if (author != null)
            {
                if (author == "")
                    problems.Add("Author field is empty.");
            }

            if (publisher != null)
            {
                if (publisher == "")
                    problems.Add("Publisher field is empty.");
            }

            if (year != null)
            {
                if (BookValidator.IsInteger((decimal)year) == false)
                    problems.Add("Year needs to be an integer.");
            }

            if (problems.Any())
                return String.Join(" ", problems.ToArray());
            else
                return String.Empty;
        }

        public static bool IsInteger(decimal bookId)
        {
            return ((bookId % 1) == 0);
        }
    }
}
