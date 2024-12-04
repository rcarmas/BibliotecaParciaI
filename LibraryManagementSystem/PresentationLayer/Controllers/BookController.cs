using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Services;
using Common.DTOs;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // Buscar libro por ID
        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            var book = _bookService.GetBookById(id);
            return book != null ? Ok(book) : NotFound("Libro no encontrado.");
        }

        // Filtrar libros por categoría
        [HttpGet("filter")]
        public IActionResult GetBooksByCategory(string category)
        {
            var books = _bookService.GetBooksByCategory(category);
            return Ok(books);
        }

        // Listar todos los libros
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = _bookService.GetAllBooks();
            return Ok(books);
        }

        // Actualizar información de un libro
        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] BookDTO bookDto)
        {
            var result = _bookService.UpdateBook(id, bookDto);
            if (!result)
                return NotFound("Libro no encontrado.");

            return Ok("Libro actualizado exitosamente.");
        }
    }
}
