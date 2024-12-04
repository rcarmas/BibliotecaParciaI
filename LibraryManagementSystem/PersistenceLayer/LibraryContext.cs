using Microsoft.EntityFrameworkCore;
using PersistenceLayer.Models;

namespace PersistenceLayer
{
    public class LibraryContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<Book> books { get; set; }
        public DbSet<Transaction> transactions { get; set; }

        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }
    }
}
