using BookCollection.Models;
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
            var books = SQLController.GetAllDBBooks();

            if (!String.IsNullOrWhiteSpace(author))
                books = books.Where(i => i.Author == author).ToList();

            if (!String.IsNullOrWhiteSpace(publisher))
                books = books.Where(i => i.Publisher == publisher).ToList();

            if (year != null)
                books = books.Where(i => i.Year == year).ToList();

            var booksJson = JsonSerializer.Serialize(books);

            return Ok(booksJson);
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
        public IActionResult PostNewBook(JObject newBook)
        {
            try
            {
                var book = Newtonsoft.Json.JsonConvert.DeserializeObject<Book>(newBook.ToString());

                return Ok($"");
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
