using Autofac.Extras.Moq;
using Booked.Controllers;
using Booked.Models.Classes;
using Booked.Utilities;
using Xunit;
using Dapper;
using System.Data;
using Moq.Dapper;
using Booked.Models.Interfaces;
using Microsoft.Extensions.Logging;
using ICSharpCode.SharpZipLib.Zip;
using Moq;

namespace Booked.Tests
{
    public class BooksControllerTests
    {

        #region MockTests

        [Fact]
        public void GetAllBooks_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDbConnection>()
                    .SetupDapper(x => x.Query<IBook>("select * from books", new DynamicParameters(), null, true, null, null))
                    .Returns(GetSampleBooks());

                var cls = mock.Create<SQLiteDataAccess>();

                var expected = GetSampleBooks();

                var actual = cls.GetAllDbBooks();

                Assert.True(actual != null);
                Assert.Equal(expected, actual);
            }

            throw new NotImplementedException();
        }

        [Fact]
        public void GetBookById_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDbConnection>()
                    .SetupDapper(x => x.Query<IBook>("SELECT * from books WHERE id = @id", new { id = 1 }, null, true, null, null))
                    .Returns(GetOneSampleBook());

                var cls = mock.Create<SQLiteDataAccess>();
                var expected = GetSampleBooks().First();

                var actual = cls.GetDbBookById(1);

                Assert.True(actual != null);
                Assert.Equal(expected, actual);
            }

            throw new NotImplementedException();
        }

        #endregion MockTests


        #region PossibleQueryProblems

        [Fact]
        public void PossibleQueryProblems_AllInfoGiven_EmptyString()
        {
            // Arrange
            var expected = (true, string.Empty);

            // Act
            var controller = new BooksController(null);
            var actual = controller.PossibleQueryProblems("Author", 2000, "Publisher");

            // Assert
            Assert.Equal(expected, actual);      
        }

        [Fact]
        public void PossibleQueryProblems_AuthorEmpty_AuthorFieldEmpty()
        {
            // Arrange
            var expected = (false, "Author field is empty.");

            // Act
            var controller = new BooksController(null);
            var actual = controller.PossibleQueryProblems("", 2000, "Publisher");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PossibleQueryProblems_PublisherEmpty_PublisherFieldEmpty()
        {
            // Arrange
            var expected = (false, "Publisher field is empty.");

            // Act
            var controller = new BooksController(null);
            var actual = controller.PossibleQueryProblems("Author", 2000, "");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("Author", 2000.02, "Publisher")]
        public void PossibleQueryProblems_YearIsDecimal_YearNotInteger(string author, decimal year, string publisher)
        {
            // Arrange
            var expected = (false, "Year needs to be an integer.");

            // Act
            var controller = new BooksController(null);
            var actual = controller.PossibleQueryProblems(author, year, publisher);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PossibleQueryProblems_AllFiltersIncorrect_ThreeErrorLines()
        {
            // Arrange
            var expected = (false, "Author field is empty. Publisher field is empty. Year needs to be an integer.");

            // Act
            var controller = new BooksController(null);
            var actual = controller.PossibleQueryProblems("", (decimal)2000.02, "");

            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion PossibleQueryProblems

        #region PossibleBookProblems

        //[Fact]
        //public void PossibleBookProblems_EmptyTitle_TitleFieldIsEmpty()
        //{
        //    // Arrange
        //    var expected = (false, "Title field is empty.");
        //    var book = new Book { Title = "", Author = "Author", Publisher = "Publisher", Year = 2000 };

        //    using (var mock = AutoMock.GetLoose())
        //    {
        //        mock.Mock<IDbConnection>()
        //            .SetupDapper(x => x.Query<IBook>("select * from books", new DynamicParameters(), null, true, null, null))
        //            .Returns(GetSampleBooks());

        //        var cls = mock.Create<SQLiteDataAccess>();

        //        actual = cls.Possible;
        //    }

        //    // Act
        //    var controller = new BooksController(null);
        //    var actual = controller.PossibleBookProblems(book);

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}

        [Fact]
        public void PossibleBookProblems_EmptyAuthor_AuthorFieldIsEmpty()
        {
            // Arrange
            (bool isValid, string problems) expected = (false, "Author field is empty.");
            var book = new Book { Title = "Title", Author = "", Publisher = "Publisher", Year = 2000 };

            // Act
            var controller = new BooksController(null);
            var actual = controller.PossibleBookProblems(book);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PossibleBookProblems_EmptyPublisher_PublisherFieldIsEmpty()
        {
            // Arrange
            var expected = (false, "Publisher field is empty.");
            var book = new Book { Title = "Title", Author = "Author", Publisher = "", Year = 2000 };

            // Act
            var controller = new BooksController(null);
            var actual = controller.PossibleBookProblems(book);

            // Assert
            Assert.Equal(expected, actual);
        }

        //[Fact]
        //public void PossibleBookProblems_IdenticalBook_BookWithSameTitleAuthorAndYearFoundAlready()
        //{
        //    // Arrange
        //    var expected = (false, "Book with same title, author and year found already.");
        //    var book = new Book { Title = "Title", Author = "Author", Publisher = "Publisher", Year = 2000 };
        //    var identicalBook = new Book { Title = "Title", Author = "Author", Publisher = "Publisher", Year = 2000 };

        //    _booksController.GetAllDbBooks = () => new List<Book> { identicalBook };

        //    // Act
        //    var actual = _booksController.PossibleBookProblems(book);

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}

        [Fact]
        public void PossibleBookProblems_NoProblems_EmptyString()
        {
            // Arrange
            var expected = (true, String.Empty);
            var book = new Book { Title = "Title", Author = "Author", Publisher = "Publisher", Year = 2000 };

            // Act
            var controller = new BooksController(null);
            var actual = controller.PossibleBookProblems(book);

            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion PossibleBookProblems

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