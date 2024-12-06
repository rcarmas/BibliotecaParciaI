using Common.DTOs;

namespace BusinessLayer.Services
{
    public interface IBookService
    {
        BookDTO GetBookById(string id);
        List<BookDTO> GetBooksByCategory(string category);
        List<BookDTO> GetAllBooks();
        bool CreateBook(BookDTO bookDto);
        bool UpdateBook(string id, BookDTO bookDto);
        bool DeleteBook(string id);

    }
}
