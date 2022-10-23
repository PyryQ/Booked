using BookCollection.Models;
using Booked.Models;
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

        [HttpGet]
        public IActionResult GetAllBooks(string? author, int? year, string? publisher)
        {
            var queryProblems = BookValidator.PossibleQueryProblems(author, year, publisher);

            if (queryProblems == String.Empty) //No problems with query parameters
            {
                var books = SQLController.GetAllDBBooks();

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
        public IActionResult GetBookById(int bookId)
        {
            if (bookId < 1)
            {
                Response.StatusCode = 400;
                return Content("Invalid book id.");
            }

            var book = SQLController.GetDBBookById(bookId);

            var bookJson = JsonSerializer.Serialize(book);

            return Ok(bookJson);
        }

        [HttpPost]
        public IActionResult PostNewBook([FromBody]Book newBook)
        {
            try
            {
                var problems = BookValidator.PossibleBookProblems(newBook);

                if (problems == String.Empty) //No problems with new book
                {
                    var latestId = SQLController.PostNewDBBook(newBook);

                    var bookId = new BookIdResponce() { id = latestId };

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

        [HttpDelete]
        public IActionResult DeleteBook(int bookId)
        {
            try
            {
                SQLController.DeleteDBBookById(bookId);

                Response.StatusCode = 204;
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
