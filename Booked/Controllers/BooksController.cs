using Booked.Models.Classes;
using Booked.Models.Interfaces;
using Booked.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace Booked.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase, IBooksController
    {
        private readonly IConfiguration _configuration;
        private readonly ExampleSettings _settings;

        public BooksController(IDatabaseDataAccess dataAccess, 
            IConfiguration configuration, 
            IOptions<ExampleSettings> settings)
        { 
            _configuration= configuration;
            _settings = (ExampleSettings?)settings;
            _dataAccess = dataAccess;
        }

        private IDatabaseDataAccess _dataAccess;

        //public BooksController(IDatabaseDataAccess dataAccess)
        //{
        //    _dataAccess = dataAccess;
        //}

        #region GET

        [HttpGet]
        public IActionResult GetAllBooks(string? author, decimal? year, string? publisher)
        {
            var queryProblems = PossibleQueryProblems(author, year, publisher);

            if (queryProblems.isValid) //No problems with query parameters
            {
                var books = _dataAccess.GetAllDbBooks();

                var value = _configuration.GetValue<List<IBook>>("ExampleSettings:ConnectionString");

                //Filter the books based on query
                if (!String.IsNullOrWhiteSpace(author))
                    books = books.Where(i => i.Author == author).ToList();

                if (!String.IsNullOrWhiteSpace(publisher))
                    books = books.Where(i => i.Publisher == publisher).ToList();

                if (year != null)
                    books = books.Where(i => i.Year == year).ToList();

                var booksJson = JsonSerializer.Serialize(books);

                return Ok(booksJson);
            }
            else
            {
                Response.StatusCode = 400;
                return Content(queryProblems.problems);
            }
        }

        [HttpGet]
        [Route("/{bookId}")]
        public IActionResult GetBookById(decimal bookId)
        {
            try
            {
                if (IsInteger(bookId))
                {
                    var book = _dataAccess.GetDbBookById((int)bookId);

                    var bookJson = JsonSerializer.Serialize(book);

                    return Ok(bookJson);
                }
                else
                {
                    Response.StatusCode = 400;
                    return Content("BookId needs to be an integer.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion GET

        #region POST

        [HttpPost]
        public IActionResult PostNewBook([FromBody] IBook newBook)
        {
            try
            {
                var problems = PossibleBookProblems(newBook);

                if (problems.isValid) //No problems with new book
                {
                    var latestId = _dataAccess.PostNewDbBook(newBook);

                    var bookId = new BookIdResponce(latestId);

                    string bookIdJson = JsonSerializer.Serialize<BookIdResponce>(bookId);

                    return Ok(bookIdJson);
                }
                else
                {
                    Response.StatusCode = 400;
                    return Content(problems.problems);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion POST

        #region DELETE

        [HttpDelete]
        public IActionResult DeleteBook(decimal bookId)
        {
            try
            {
                if (IsInteger(bookId))
                {
                    _dataAccess.DeleteDbBookById((int)bookId);

                    Response.StatusCode = 204;
                    return Content(string.Empty);
                }
                else
                {
                    Response.StatusCode = 400;
                    return Content("BookId needs to be an integer.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion DELETE

        #region validators

        /// <summary>
        /// Collects problems with the book and adds it to a string. Returns list of possible problems.
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public (bool isValid, string problems) PossibleBookProblems(IBook book)
        {
            var problems = new List<string>();

            if (String.IsNullOrEmpty(book.Title))
                problems.Add("Title field is empty.");

            if (String.IsNullOrEmpty(book.Author))
                problems.Add("Author field is empty.");

            if (String.IsNullOrEmpty(book.Publisher))
                problems.Add("Publisher field is empty.");


            var books = _dataAccess.GetAllDbBooks();

            var identicalBooksFound = books.Where(i => i.Title == book.Title && i.Year == book.Year && i.Author == book.Author).Any();

            if (identicalBooksFound)
                problems.Add("Book with same title, author and year found already.");

            if (problems.Any())
                return (false, String.Join(" ", problems.ToArray()));
            else
                return (true, String.Empty);
        }

        /// <summary>
        /// Collects problems with book query filters to a string. Return empty string if no problems found.
        /// </summary>
        /// <param name="author"></param>
        /// <param name="year"></param>
        /// <param name="publisher"></param>
        /// <returns></returns>
        public (bool isValid, string problems) PossibleQueryProblems(string? author, decimal? year, string? publisher)
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
                if (IsInteger((decimal)year) == false)
                    problems.Add("Year needs to be an integer.");
            }

            if (problems.Any())
                return (false, String.Join(" ", problems.ToArray()));
            else
                return (true, String.Empty);
        }

        public bool IsInteger(decimal bookId)
        {
            return ((bookId % 1) == 0);
        }

        #endregion validators
    }
}
