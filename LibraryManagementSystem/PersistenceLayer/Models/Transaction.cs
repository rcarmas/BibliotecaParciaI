namespace PersistenceLayer.Models
{
    public class Transaction
    {
        public int id { get; set; }
        public int userid { get; set; }
        public int bookid { get; set; }
        public DateTime? borrowdate { get; set; }  // Asegúrate de que sea nullable
        public DateTime? returndate { get; set; }  // Asegúrate de que sea nullable
        public string status { get; set; }

        public decimal? fine { get; set; }  // Propiedad nullable para la multa

        public User User { get; set; }
        public Book Book { get; set; }
    }
}
