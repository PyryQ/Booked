using Newtonsoft.Json;


namespace BookCollection.Models
{
    public class Book
    {
        /// <summary>
        /// Int identifier for the book
        /// </summary>
        private int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public int Year { get; set; }

        public string Publisher { get; set; }

        public string? Description { get; set; }
    }
}
