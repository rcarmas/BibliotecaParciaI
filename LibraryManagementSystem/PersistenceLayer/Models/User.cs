namespace PersistenceLayer.Models
{
    public class User
    {
        public Guid id { get; set; }  
        public string name { get; set; }
        public string email { get; set; }
        public string usertype { get; set; }
        public string status { get; set; }
        public string password { get; set; } 
    }
}
