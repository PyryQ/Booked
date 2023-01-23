using Booked.Models.Interfaces;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Booked.Models.Classes
{
    public class Book : IBook
    {
        /// <summary>
        /// Int identifier for the book
        /// </summary>
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public int Year { get; set; }

        public string Publisher { get; set; }

        public string? Description { get; set; }
    }
}
