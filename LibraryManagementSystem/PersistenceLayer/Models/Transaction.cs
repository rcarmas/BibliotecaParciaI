namespace PersistenceLayer.Models
{
    public class Transaction
    {
        public Guid id { get; set; }
        public Guid userid { get; set; }
        public string bookid { get; set; }
        public DateTime? borrowdate { get; set; }  
        public DateTime? returndate { get; set; }  
        public string status { get; set; }

        public decimal? fine { get; set; }  
        public User User { get; set; }
        public Book Book { get; set; }
    }
}
