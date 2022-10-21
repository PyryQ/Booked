using BookCollection.Models;
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

namespace Booked.Controllers
{
    public class SQLController
    {
        private static string LoadConnectionString(string id ="Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public static void PostNewDBBook(Book book)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                if (String.IsNullOrEmpty(book.Title))
                    throw new Exception("Author field is empty");

                if (String.IsNullOrEmpty(book.Author))
                    throw new Exception("Author field is empty");

                if (String.IsNullOrEmpty(book.Publisher))
                    throw new Exception("Author field is empty");

                if (book.Year != null)
                    throw new Exception("Author field is empty");

                try
                {
                    cnn.Execute("INSERT into books (title, author, year, publisher, description) value (@Title, @Author, @Year, @Publisher, @Description)", book);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        #region GET

        /// <summary>
        /// 
        /// </summary>
        /// <param name="author"></param>
        /// <param name="publisher"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<Book> GetDBBooks(string author, string publisher, int year)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<Book>("SELECT * from books", new DynamicParameters());

                return output.ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="author"></param>
        /// <param name="publisher"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Book GetDBBookById(int bookId)
        {
            using (var cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<Book>("SELECT * from books WHERE id = @id", new { id = bookId });

                return output.First();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Book> GetAllDBBooks()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<Book>("select * from books", new DynamicParameters());

                return output.ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public static List<Book> GetAllDBBooks()
        //{
        //    using (IDbConnection cnn = new SqliteConnection(LoadConnectionString()))
        //    {
        //        var output = cnn.Query<Book>("select * from books", new DynamicParameters());

        //        return output.ToList();
        //    }
        //}

        #endregion GET

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public static void DeleteDBBookById(int bookId)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<Book>("DELETE * from books WHERE id = @id", new { id = bookId });
            }
        }
    }
}
