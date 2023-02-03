using Booked.Models.Interfaces;

namespace Booked.Utilities
{
    public interface IDatabaseDataAccess
    {
        void DeleteDbBookById(int bookId);

        List<IBook> GetAllDbBooks();

        IBook GetDbBookById(int bookId);

        int PostNewDbBook(IBook book);
    }
}