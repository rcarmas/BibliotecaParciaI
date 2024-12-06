using Common.DTOs;
using PersistenceLayer;
using PersistenceLayer.Models;

namespace BusinessLayer.Services
{
    public class BookService : IBookService
    {
        private readonly LibraryContext _context;

        public BookService(LibraryContext context)
        {
            _context = context;
        }

        public bool CreateBook(BookDTO bookDto)
        {
            var book = new Book
            {
                id = bookDto.Id,  
                title = bookDto.Title,
                author = bookDto.Author,
                category = bookDto.Category,
                availability = bookDto.Availability,
                location = bookDto.Location
            };

            _context.books.Add(book);
            _context.SaveChanges();
            return true;
        }
        public BookDTO GetBookById(string id)
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

        public bool UpdateBook(string id, BookDTO bookDto)
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
        public bool DeleteBook(string id)
        {
            var book = _context.books.FirstOrDefault(b => b.id == id);  
            if (book == null)
                return false;  

            _context.books.Remove(book);  
            _context.SaveChanges();  

            return true; 
        }
    }
}
