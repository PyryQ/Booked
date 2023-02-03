using Booked.Models.Classes;
using Booked.Models.Interfaces;
using Dapper;
using System.Data;
using System.Data.SQLite;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace Booked.Utilities
{
    public class PostgresDataAccess : IDatabaseDataAccess
    {
        private readonly BookDataContext context;

        public PostgresDataAccess(BookDataContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Returns connections string configured in App.config based on id given.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        #region GET

        /// <summary>
        /// Returns all the books from the database
        /// </summary>
        /// <returns></returns>
        public List<IBook> GetAllDbBooks()
        {
            try
            {
                var books = context.Books.ToList();

                return books.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Returns book by id
        /// </summary>
        /// <param name="author"></param>
        /// <param name="publisher"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IBook GetDbBookById(int bookId)
        {
            try
            {
                var books = context.Books.Where(b => b.Id == bookId);

                return books.First();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion GET

        #region POST

        /// <summary>
        /// Post a new book into the database. Returns the latest id.
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public int PostNewDbBook(IBook book)
        {
            try
            {
                using (SQLiteConnection con = new SQLiteConnection(LoadConnectionString()))
                {
                    con.Execute("INSERT INTO books (title, author, year, publisher, description) VALUES (@Title, @Author, @Year, @Publisher, @Description)", book);

                    long lastId = (long)con.ExecuteScalar("SELECT MAX(id) FROM books");

                    return (int)lastId;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion POST

        #region DELETE

        /// <summary>
        /// Delete book based on given id
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public void DeleteDbBookById(int bookId)
        {
            try
            {
                using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
                {
                    var output = con.Query<IBook>("DELETE FROM books WHERE id = @id", new { id = bookId });
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