using Newtonsoft.Json;


namespace BookCollection.Models
{
    public class Book
    {
        /// <summary>
        /// Int identifier for the book
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("publisher")]
        public string Publisher { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
