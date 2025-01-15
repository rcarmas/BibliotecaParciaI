namespace PersistenceLayer.Models
{
    public class Book
    {
        public string id { get; set; }  
        public string title { get; set; }
        public string author { get; set; }
        public string category { get; set; }
        public string availability { get; set; }
        public string location { get; set; }

        public ICollection<Transaction> transactions { get; set; }
    }
}
