﻿using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Services;
using Common.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    [Route("Book")]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // Acción para listar todos los libros
        [HttpGet]
        public IActionResult Index(string searchTerm)
        {
        
            var books = _bookService.GetAllBooks();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                books = books.Where(b =>
                    b.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    b.Category.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            ViewData["SearchTerm"] = searchTerm; 
            return View(books);
        }

        // Acción para mostrar el formulario de creación de libro
        [HttpGet("Create")]
        public IActionResult Create()
        {
            var book = new BookDTO(); 
            return View(book);  
        }

        // Acción para crear un libro (POST)
        [HttpPost("Create")]
        public IActionResult Create(BookDTO bookDto)
        {
            if (ModelState.IsValid)
            {
                // Generar un UUID para el id
                var uuid = Guid.NewGuid().ToString();
                bookDto.Id = uuid;
 
                var result = _bookService.CreateBook(bookDto);
                if (!result)
                {
                    return BadRequest("No se pudo crear el libro.");
                }

                return RedirectToAction("Index");  
            }

            return View(bookDto);  
        }

        // Acción para mostrar el formulario de edición de libro
        [HttpGet("Edit/{id}")]
        public IActionResult Edit(string id)
        {
            var book = _bookService.GetBookById(id);
            if (book == null)
            {
                return NotFound("Libro no encontrado.");
            }

            return View(book); 
        }
        // Acción para actualizar un libro (POST)
        [HttpPost("Edit/{id}")]
        public IActionResult Edit(string id, BookDTO bookDto)
        {
            if (ModelState.IsValid)
            {
                var result = _bookService.UpdateBook(id, bookDto);
                if (!result)
                {
                    return NotFound("Libro no encontrado.");
                }

                return RedirectToAction("Index");  
            }

            return View(bookDto);  // Si hay errores, vuelve a la vista con el libro modificado
        }

        // Acción para eliminar un libro
        [HttpGet("Delete/{id}")]
        public IActionResult Delete(string id)
        {
            var result = _bookService.DeleteBook(id);
            if (!result)
            {
                return NotFound("Libro no encontrado.");
            }

            return RedirectToAction(nameof(Index));
        }

     
    }
}
