using Booked.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Booked.Controllers
{
    public interface IBooksController
    {
        IActionResult DeleteBook(decimal bookId);

        IActionResult GetAllBooks(string? author, decimal? year, string? publisher);

        IActionResult GetBookById(decimal bookId);

        IActionResult PostNewBook([FromBody] IBook newBook);

        public (bool isValid, string problems) PossibleQueryProblems(string? author, decimal? year, string? publisher);

        public (bool isValid, string problems) PossibleBookProblems(IBook book);
    }
}