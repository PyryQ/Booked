using Booked.Models.Classes;
using Booked.Models.Interfaces;
using Booked.SupportClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace Booked.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;

        public BooksController(ILogger<BooksController> logger)
        {
            _logger = logger;
        }

        #region GET

        [HttpGet]
        public IActionResult GetAllBooks(string? author, decimal? year, string? publisher)
        {
            var queryProblems = BookValidator.PossibleQueryProblems(author, year, publisher);

            if (queryProblems == String.Empty) //No problems with query parameters
            {
                var books = SQLiteController.GetAllDBBooks();

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
                return Content(queryProblems);
            }
        }

        [HttpGet]
        [Route("/{bookId}")]
        public IActionResult GetBookById(decimal bookId)
        {
            try
            {
                if (BookValidator.IsInteger(bookId))
                {
                    var book = SQLiteController.GetDBBookById((int)bookId);

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
        public IActionResult PostNewBook([FromBody]IBook newBook)
        {
            try
            {
                var problems = BookValidator.PossibleBookProblems(newBook);

                if (String.IsNullOrWhiteSpace(problems)) //No problems with new book
                {
                    var latestId = SQLiteController.PostNewDBBook(newBook);

                    var bookId = new BookIdResponce(latestId);

                    string bookIdJson = JsonSerializer.Serialize<BookIdResponce>(bookId);

                    return Ok(bookIdJson);
                }
                else 
                {
                    Response.StatusCode = 400;
                    return Content(problems);
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
                if (BookValidator.IsInteger(bookId))
                {
                    SQLiteController.DeleteDBBookById((int)bookId);

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
    }
}
