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
    public class BooksAPIController : ControllerBase
    {
        private readonly ILogger<BooksAPIController> _logger;

        public BooksAPIController(ILogger<BooksAPIController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllBooks(string? author, int? year, string? publisher)
        {
            var queryProblems = ValidationFunctions.PossibleQueryProblems(author, year, publisher);

            if (queryProblems == String.Empty)
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
                throw new Exception("Invalid bookId");

            var book = SQLController.GetDBBookById(bookId);

            var bookJson = JsonSerializer.Serialize(book);

            return Ok(bookJson);
        }

        [HttpPost]
        public IActionResult PostNewBook([FromBody]BookPostInfo newBook)
        {
            try
            {
                var problems = ValidationFunctions.PossibleBookProblems(newBook);

                if (problems == String.Empty)
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

                return Ok($"");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
