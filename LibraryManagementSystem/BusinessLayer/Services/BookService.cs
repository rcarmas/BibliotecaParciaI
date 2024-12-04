using Common.DTOs;
using PersistenceLayer;

namespace BusinessLayer.Services
{
    public class BookService : IBookService
    {
        private readonly LibraryContext _context;

        public BookService(LibraryContext context)
        {
            _context = context;
        }

        public BookDTO GetBookById(int id)
        {
            var book = _context.books.Find(id);
            return book == null ? null : new BookDTO
            {
                Id = book.id,
                Title = book.title,
                Author = book.author,
                Category = book.category,
                Availability = book.availability,
                Location = book.location
            };
        }

        public List<BookDTO> GetBooksByCategory(string category)
        {
            return _context.books
                .Where(b => b.category == category)
                .Select(b => new BookDTO
                {
                    Id = b.id,
                    Title = b.title,
                    Author = b.author,
                    Category = b.category,
                    Availability = b.availability,
                    Location = b.location
                }).ToList();
        }

        public List<BookDTO> GetAllBooks()
        {
            return _context.books
                .Select(b => new BookDTO
                {
                    Id = b.id,
                    Title = b.title,
                    Author = b.author,
                    Category = b.category,
                    Availability = b.availability,
                    Location = b.location
                }).ToList();
        }

        public bool UpdateBook(int id, BookDTO bookDto)
        {
            var book = _context.books.Find(id);
            if (book == null)
                return false;

            book.title = bookDto.Title;
            book.author = bookDto.Author;
            book.category = bookDto.Category;
            book.availability = bookDto.Availability;
            book.location = bookDto.Location;

            _context.SaveChanges();
            return true;
        }
    }
}
