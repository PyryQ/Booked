using Booked.Models;
using Booked.Models.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace Booked.Utilities
{
    public class SQLiteDataAccess : IDatabaseDataAccess
    {
        IDbConnection _connection;

        public SQLiteDataAccess()
        {
            _connection = new SQLiteConnection(LoadConnectionString());
        }

        /// <summary>
        /// Returns connections string configured in App.config based on id given.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        #region GET

        /// <summary>
        /// Returns all the books from the database
        /// </summary>
        /// <returns></returns>
        public List<IBook> GetAllDBBooks()
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    var output = cnn.Query<IBook>("select * from books", new DynamicParameters());
                    return output.ToList();
                }
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
        public IBook GetDBBookById(int bookId)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    var output = cnn.Query<IBook>("select * from books WHERE id = @id", new DynamicParameters());
                    return output.First();
                }
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
        public int PostNewDBBook(IBook book)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    _connection.Execute("INSERT INTO books (title, author, year, publisher, description) VALUES (@Title, @Author, @Year, @Publisher, @Description)", book);

                    long lastId = (long)_connection.ExecuteScalar("SELECT MAX(id) FROM books");

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
        public void DeleteDBBookById(int bookId)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    var output = _connection.Query<IBook>("DELETE FROM books WHERE id = @id", new { id = bookId });
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
