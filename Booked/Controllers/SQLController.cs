﻿using BookCollection.Models;
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

        #region GET

        /// <summary>
        /// Returns book by id
        /// </summary>
        /// <param name="author"></param>
        /// <param name="publisher"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Book GetDBBookById(int bookId)
        {
            try
            {
                using (var con = new SQLiteConnection(LoadConnectionString()))
                {
                    var output = con.Query<Book>("SELECT * from books WHERE id = @id", new { id = bookId });

                    return output.First();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Returns all the books from the database
        /// </summary>
        /// <returns></returns>
        public static List<Book> GetAllDBBooks()
        {
            try
            {
                using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
                {
                    var output = con.Query<Book>("select * from books", new DynamicParameters());

                    return output.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion GET

        /// <summary>
        /// Post a new book into the database. Returns the latest id.
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public static int PostNewDBBook(BookPostInfo book)
        {
            try
            {
                using (SQLiteConnection con = new SQLiteConnection(LoadConnectionString()))
                {
                    con.Execute("INSERT INTO books (title, author, year, publisher, description) VALUES (@Title, @Author, @Year, @Publisher, @Description)", book);

                    string sql = @"SELECT MAX(id) FROM books";
                    long lastId = (long)con.ExecuteScalar(sql);

                    return (int)lastId;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete book based on given id
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public static void DeleteDBBookById(int bookId)
        {
            try
            {
                using (IDbConnection con = new SQLiteConnection(LoadConnectionString()))
                {
                    var output = con.Query<Book>("DELETE FROM books WHERE id = @id", new { id = bookId });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
