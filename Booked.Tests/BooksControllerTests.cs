using Booked.Controllers;
using Booked.Models.Classes;
using Booked.Utilities;

using Xunit;

namespace Booked.Tests
{
    public class BooksControllerTests
    {

        IBooksController _booksController;

        public BooksControllerTests(BooksController booksController)
        {
            _booksController = booksController;
        }

        #region PossibleQueryProblems

        [Fact]
        public void PossibleQueryProblems_AllInfoGiven_EmptyString()
        {
            // Arrange
            var expected = (true, string.Empty);

            // Act
            var actual = _booksController.PossibleQueryProblems("Author", 2000, "Publisher");

            // Assert
            Assert.Equal(expected, actual);      
        }

        [Fact]
        public void PossibleQueryProblems_AuthorEmpty_AuthorFieldEmpty()
        {
            // Arrange
            var expected = (false, "Author field is empty.");

            // Act
            var actual = _booksController.PossibleQueryProblems("", 2000, "Publisher");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PossibleQueryProblems_PublisherEmpty_PublisherFieldEmpty()
        {
            // Arrange
            var expected = (false, "Publisher field is empty.");

            // Act
            var actual = _booksController.PossibleQueryProblems("Author", 2000, "");

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
            var actual = _booksController.PossibleQueryProblems(author, year, publisher);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PossibleQueryProblems_AllFiltersIncorrect_ThreeErrorLines()
        {
            // Arrange
            var expected = (false, "Author field is empty. Publisher field is empty. Year needs to be an integer.");

            // Act
            var actual = _booksController.PossibleQueryProblems("", (decimal)2000.02, "");

            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion PossibleQueryProblems

        #region PossibleBookProblems

        [Fact]
        public void PossibleBookProblems_EmptyTitle_TitleFieldIsEmpty()
        {
            // Arrange
            var expected = (false, "Title field is empty");
            var book = new Book { Title = "", Author = "Author", Publisher = "Publisher", Year = 2000 };

            // Act
            var actual = _booksController.PossibleBookProblems(book);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PossibleBookProblems_EmptyAuthor_AuthorFieldIsEmpty()
        {
            // Arrange
            var expected = (false, "Author field is empty.");
            var book = new Book { Title = "Title", Author = "", Publisher = "Publisher", Year = 2000 };

            // Act
            var actual = _booksController.PossibleBookProblems(book);

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
            var actual = _booksController.PossibleBookProblems(book);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PossibleBookProblems_IdenticalBook_BookWithSameTitleAuthorAndYearFoundAlready()
        {
            // Arrange
            var expected = (false, "Book with same title, author and year found already.");
            var book = new Book { Title = "Title", Author = "Author", Publisher = "Publisher", Year = 2000 };
            var identicalBook = new Book { Title = "Title", Author = "Author", Publisher = "Publisher", Year = 2000 };

            SQLiteDataAccess.GetAllDBBooks = () => new List<Book> { identicalBook };

            // Act
            var actual = _booksController.PossibleBookProblems(book);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PossibleBookProblems_NoProblems_EmptyString()
        {
            // Arrange
            var book = new Book { Title = "Title", Author = "Author", Publisher = "Publisher", Year = 2000 };

            // Act
            var result = _booksController.PossibleBookProblems(book);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        #endregion PossibleBookProblems
    }
}