using Common.DTOs;

namespace BusinessLayer.Services
{
    public interface IBookService
    {
        BookDTO GetBookById(int id);
        List<BookDTO> GetBooksByCategory(string category);
        List<BookDTO> GetAllBooks();
        bool UpdateBook(int id, BookDTO bookDto);
    }
}
