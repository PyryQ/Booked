using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booked;
using Booked.Controllers;
using Booked.Models.Classes;
using Booked.Models.Interfaces;
using global::Booked.SupportClasses;
using Xunit;

namespace Booked.Tests
{
    public class ValidatorTests
    {
        #region PossibleQueryProblems

        [Fact]
        public void PossibleQueryProblems_AllInfoGiven_EmptyString()
        {
            // Arrange
            string expected = string.Empty;

            // Act
            string actual = BookValidator.PossibleQueryProblems("Author", 2000, "Publisher");

            // Assert
            Assert.Equal(expected, actual);      
        }

        [Fact]
        public void PossibleQueryProblems_AuthorEmpty_AuthorFieldEmpty()
        {
            // Arrange
            string expected = "Author field is empty.";

            // Act
            string actual = BookValidator.PossibleQueryProblems("", 2000, "Publisher");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PossibleQueryProblems_PublisherEmpty_PublisherFieldEmpty()
        {
            // Arrange
            string expected = "Publisher field is empty.";

            // Act
            string actual = BookValidator.PossibleQueryProblems("Author", 2000, "");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("Author", 2000.02, "Publisher")]
        public void PossibleQueryProblems_YearIsDecimal_YearNotInteger(string author, decimal year, string publisher)
        {
            // Arrange
            string expected = "Year needs to be an integer.";

            // Act
            string actual = BookValidator.PossibleQueryProblems(author, year, publisher);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PossibleQueryProblems_AllFiltersIncorrect_ThreeErrorLines()
        {
            // Arrange
            string expected = "Author field is empty. Publisher field is empty. Year needs to be an integer.";

            // Act
            string actual = BookValidator.PossibleQueryProblems("", (decimal)2000.02, "");

            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion PossibleQueryProblems

        #region PossibleBookProblems

        [Fact]
        public void PossibleBookProblems_EmptyTitle_TitleFieldIsEmpty()
        {
            // Arrange
            var book = new Book { Title = "", Author = "Author", Publisher = "Publisher", Year = 2000 };

            // Act
            var result = BookValidator.PossibleBookProblems(book);

            // Assert
            Assert.Equal("Title field is empty.", result);
        }

        [Fact]
        public void PossibleBookProblems_EmptyAuthor_AuthorFieldIsEmpty()
        {
            // Arrange
            var book = new Book { Title = "Title", Author = "", Publisher = "Publisher", Year = 2000 };

            // Act
            var result = BookValidator.PossibleBookProblems(book);

            // Assert
            Assert.Equal("Author field is empty.", result);
        }

        [Fact]
        public void PossibleBookProblems_EmptyPublisher_PublisherFieldIsEmpty()
        {
            // Arrange
            var book = new Book { Title = "Title", Author = "Author", Publisher = "", Year = 2000 };

            // Act
            var result = BookValidator.PossibleBookProblems(book);

            // Assert
            Assert.Equal("Publisher field is empty.", result);
        }

        //[Fact]
        //public void PossibleBookProblems_IdenticalBook_BookWithSameTitleAuthorAndYearFoundAlready()
        //{
        //    // Arrange
        //    var book = new Book { Title = "Title", Author = "Author", Publisher = "Publisher", Year = 2000 };
        //    var identicalBook = new Book { Title = "Title", Author = "Author", Publisher = "Publisher", Year = 2000 };

        //    SQLController.GetAllDBBooks = () => new List<Book> { identicalBook };

        //    // Act
        //    var result = BookValidator.PossibleBookProblems(book);

        //    // Assert
        //    Assert.Equal("Book with same title, author and year found already.", result);
        //}

        [Fact]
        public void PossibleBookProblems_NoProblems_EmptyString()
        {
            // Arrange
            var book = new Book { Title = "Title", Author = "Author", Publisher = "Publisher", Year = 2000 };

            // Act
            var result = BookValidator.PossibleBookProblems(book);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        #endregion PossibleBookProblems
    }
}