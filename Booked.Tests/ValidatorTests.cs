using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booked;
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
        public void PossibleQueryProblems_AllInfoGiven_ReturnsEmptyString()
        {
            // Arrange
            string expected = string.Empty;

            // Act
            string actual = BookValidator.PossibleQueryProblems("Author", 2000, "Publisher");

            // Assert
            Assert.Equal(expected, actual);      
        }

        [Fact]
        public void PossibleQueryProblems_AuthorEmpty_ReturnsErrorString()
        {
            // Arrange
            string expected = "Author field is empty.";

            // Act
            string actual = BookValidator.PossibleQueryProblems("", 2000, "Publisher");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PossibleQueryProblems_PublisherEmpty_ReturnsErrorString()
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
        public void PossibleQueryProblems_YearIsDecimal_ReturnsErrorString(string author, decimal year, string publisher)
        {
            // Arrange
            string expected = "Year needs to be an integer.";

            // Act
            string actual = BookValidator.PossibleQueryProblems(author, year, publisher);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PossibleQueryProblems_AllFiltersIncorrect_ReturnsErrorString()
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

        //[Fact]
        //public void PossibleBookProblems_AllInfoGiven_ReturnsEmptyString()
        //{
        //    // Arrange
        //    string expected = string.Empty;

        //    // Act
        //    Book book = new Book() { Title ="Title", Author = "Author", };

        //    string actual = BookValidator.PossibleQueryProblems("", (decimal)2000.02, "");

        //    // Assert
        //}

        #endregion PossibleBookProblems
    }
}