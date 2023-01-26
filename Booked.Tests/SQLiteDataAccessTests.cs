using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Booked.Models.Classes;
using Booked.Models.Interfaces;
using Dapper;
using Xunit;
using System.Data;
using Moq;
using Moq.Dapper;
using System.Net;
using Booked.Utilities;

namespace Booked.Tests
{
    public class SQLiteDataAccessTests
    {

        //[Fact]
        //public void GetAllBooks_ValidCall()
        //{
        //    using (var mock = AutoMock.GetLoose())
        //    {
        //        mock.Mock<IDbConnection>()
        //            .SetupDapper(x => x.Query<IBook>("select * from books", new DynamicParameters(), null, true, null, null))
        //            .Returns(GetSampleBooks());

        //        var cls = mock.Create<SQLiteDataAccess>

        //        var expected = GetSampleBooks();

        //        var actual = SQLiteDataAccess.GetAllDBBooks();

        //        Assert.True(actual != null);
        //        Assert.Equal(expected, actual);
        //    }

        //    throw new NotImplementedException();
        //}

        //[Fact]
        //public void GetBookById_ValidCall()
        //{
        //    using (var mock = AutoMock.GetLoose())
        //    {
        //        mock.Mock<IDbConnection>()
        //            .SetupDapper(x => x.Query<IBook>("SELECT * from books WHERE id = @id", new { id = 1 }, null, true, null, null))
        //            .Returns(GetOneSampleBook());

        //        var cls = mock.Create<SQLiteController>();
        //        var expected = GetSampleBooks().First();

        //        var actual = cls.GetDBBookById(1);

        //        Assert.True(actual != null);
        //        Assert.Equal(expected, actual);
        //    }

        //    throw new NotImplementedException();
        //}

        private List<IBook> GetSampleBooks()
        {
            List<IBook> output = new List<IBook>
            {
                new Book{ Id = 1, Title = "Title", Author = "Author", Publisher = "Publisher", Year = 2000},
                new Book{ Id = 2, Title = "Fault in our stars", Author = "Juhn Green", Publisher = "Penguin", Year = 2000}
            };

            return output;
        }

        private List<IBook> GetOneSampleBook()
        {
            List<IBook> output = new List<IBook>
            {
                new Book{ Id = 1, Title = "Title", Author = "Author", Publisher = "Publisher", Year = 2000},          
            };

            return output;
        }

    }
}
