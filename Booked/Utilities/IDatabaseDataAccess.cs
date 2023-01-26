using Booked.Models.Interfaces;

namespace Booked.Utilities
{
    public interface IDatabaseController
    {
        void DeleteDBBookById(int bookId);
        List<IBook> GetAllDBBooks();
        IBook GetDBBookById(int bookId);
        int PostNewDBBook(IBook book);
    }
}