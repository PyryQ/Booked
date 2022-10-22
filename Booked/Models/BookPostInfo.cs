using Newtonsoft.Json;


namespace BookCollection.Models
{
    public class BookPostInfo
    {
        public string Title { get; set; }

        public string Author { get; set; }

        public int Year { get; set; }

        public string Publisher { get; set; }

        public string? Description { get; set; }
    }
}
