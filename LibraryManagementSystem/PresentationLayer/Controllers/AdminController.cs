using BusinessLayer.Services;
using Common.DTOs;
using Common.ViewModels;
using CoreWCF.IdentityModel.Protocols.WSTrust;
using LibraryManagementSystem.Services.Soap;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using PersistenceLayer;
using PersistenceLayer.Models;
using System.Text;
using System.Xml.Linq;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        
        private readonly ILibraryService _libraryService;
        private readonly ITransactionService _transactionService;
        private readonly IBookService _bookService;
        private readonly IUserService _userService;
        private readonly LibraryContext _context;

        public AdminController(ILibraryService libraryService,
                               ITransactionService transactionService,
                               IBookService bookService,
                               IUserService userService)
        {
            _libraryService = libraryService;
            _transactionService = transactionService;
            _bookService = bookService;
            _userService = userService;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Dashboard()
        {
            return View();  
        }
        [Authorize(Roles = "Admin")]
        // Acción para la gestión de libros
        public IActionResult ManageBooks()
        {
            return View();  
        }
        [Authorize(Roles = "Admin")]
        // Acción para la gestión de usuarios
        public IActionResult ManageUsers(string searchTerm)
        {
            var users = _libraryService.GetAllUsers();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                users = users.Where(u => u.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            ViewData["SearchTerm"] = searchTerm;

            return View(users);
        }

        // Acción para activar un usuario
        public IActionResult ActivateUser(Guid userId)
        {
            var result = _libraryService.ActivateUser(userId); // Crear este método en el servicio para activar el usuario
            ViewBag.Message = result;
            return RedirectToAction("ManageUsers");
        }

        // Acción para suspender un usuario
        public IActionResult SuspendUser(Guid userId)
        {
            var result = _libraryService.SuspendUser(userId); // Crear este método en el servicio para suspender el usuario
            ViewBag.Message = result;
            return RedirectToAction("ManageUsers");
        }
        // Acción para editar un usuario
        public IActionResult EditUser(Guid userId)
        {
            var user = _libraryService.GetAllUsers().FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Usuario no encontrado.";
                return RedirectToAction("ManageUsers"); 
            }

            return View(user); 
        }
        [HttpPost]
        public IActionResult EditUser(Guid userId, string name, string email, string status, string userType)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(status) || string.IsNullOrEmpty(userType))
            {
                TempData["ErrorMessage"] = "Todos los campos deben ser completados.";
                return RedirectToAction("EditUser", new { userId }); 
            }

            var result = _libraryService.EditUser(userId, name, email, status, userType);

            if (result.Contains("editado correctamente"))
            {
                TempData["SuccessMessage"] = "Usuario editado correctamente.";
            }
            else
            {
                TempData["ErrorMessage"] = result;
            }

            return RedirectToAction("ManageUsers"); 
        }
        // Acción para eliminar un usuario
        public IActionResult DeleteUser(Guid userId)
        {
            var result = _libraryService.DeleteUser(userId);  // Método que eliminaria el usuario
            ViewBag.Message = result;
            return RedirectToAction("ManageUsers");
        }
        // Acción para registrar préstamos y devoluciones
        public IActionResult ManageTransactions()
        {
            // Obtener todas las transacciones
            var transactions = _libraryService.GetTransactions();  

            // Deberíamos mapear las transacciones a TransactionDTO
            var transactionDTOs = transactions.Select(t => new TransactionDTO
            {
                Id = t.Id,
                UserId = t.UserId,
                BookId = t.BookId,
                BorrowDate = t.BorrowDate,
                ReturnDate = t.ReturnDate,
                Status = t.Status,
                Fine = t.Fine,
                UserName = t.UserName,  
                BookTitle = t.BookTitle 
            }).ToList();

            return View(transactionDTOs);  
        }
        // Acción para registrar un préstamo
        [HttpPost]
        public IActionResult RegisterLoan(Guid userId, string bookId, string status, DateTime borrowDate, DateTime? returnDate, decimal? fine)
        {
            var book = _bookService.GetBookById(bookId);

            if (book == null)
            {
                ViewBag.Message = "Error: El libro no existe.";
                return RegisterLoan();
            }

            if (book.Availability != "Disponible")
            {
                ViewBag.Message = "Error: El libro no está disponible.";
                return RegisterLoan();
            }

            var result = _transactionService.RegisterLoan(userId, bookId, status, borrowDate, returnDate, fine);
            ViewBag.Message = result;

            return RedirectToAction("ManageTransactions");
        }
        // Acción GET para mostrar el formulario de préstamo
        [HttpGet]
        public IActionResult RegisterLoan()
        {
            var users = _userService.GetAllUsers();
            var books = _bookService.GetAllBooks().ToList();  // Conversión a List<BookDTO>

            var model = new LoanViewModel
            {
                Users = users,
                Books = books
            };

            return View(model);
        }
        public IActionResult RegisterReturn(Guid transactionId)
        {
            var result = _transactionService.RegisterReturn(transactionId); 
            return RedirectToAction("ManageTransactions"); 
        }
        [Authorize(Roles = "Admin")]
        // Acción para generar reportes
        public IActionResult Reports()
        {
            var users = _userService.GetAllUsers(); // Obtén la lista de usuarios desde el servicio
            ViewBag.Users = users; // Enviar a la vista
            return View();
        }
        // Acción para generar reporte de préstamos activos
        [HttpGet]
        public IActionResult GenerateActiveLoansReport(DateTime? startDate, DateTime? endDate, bool exportToCsv = true)
        {
            var report = _transactionService.GenerateActiveLoansReport(startDate, endDate);

            if (exportToCsv)
            {
                var csvData = new StringBuilder();
                csvData.AppendLine("Usuario,Libro,Fecha de Prestamo,Fecha de Devolucion,Estado,Multa");

                foreach (var transaction in report)
                {
                    csvData.AppendLine($"{transaction.UserName}," +
                                       $"{transaction.BookTitle}," +
                                       $"{transaction.BorrowDate:yyyy-MM-dd}," +
                                       $"{(transaction.ReturnDate.HasValue ? transaction.ReturnDate.Value.ToString("yyyy-MM-dd") : "No devuelto")}," +
                                       $"{transaction.Status}," +
                                       $"{(transaction.Fine.HasValue ? transaction.Fine.Value.ToString("F2") : "N/A")}");
                }

                var fileName = "ReportePrestamos.csv";
                var fileBytes = Encoding.UTF8.GetBytes(csvData.ToString());
                return File(fileBytes, "text/csv", fileName);
            }

            ViewBag.Report = report;
            return View();
        }
        // Acción para generar reporte de historial de usuario
        [HttpGet]
        public IActionResult GenerateUserHistoryReport(Guid userId, bool exportToCsv = true)
        {
            var report = _transactionService.GenerateUserHistoryReport(userId);

            if (exportToCsv)
            {
                var csvData = new StringBuilder();
                csvData.AppendLine("Usuario,Libro,Fecha de Prestamo,Fecha de Devolucion,Estado,Multa");

                string userName = string.Empty;

                foreach (var transaction in report)
                {
                    if (string.IsNullOrEmpty(userName))
                    {
                        userName = transaction.UserName;
                    }

                    csvData.AppendLine($"{transaction.UserName}," +
                                       $"{transaction.BookTitle}," +
                                       $"{transaction.BorrowDate:yyyy-MM-dd}," +
                                       $"{(transaction.ReturnDate.HasValue ? transaction.ReturnDate.Value.ToString("yyyy-MM-dd") : "No devuelto")}," +
                                       $"{transaction.Status}," +
                                       $"{(transaction.Fine.HasValue ? transaction.Fine.Value.ToString("F2") : "N/A")}");
                }

                // Asegúrate de que el nombre del archivo sea seguro para un sistema de archivos
                var sanitizedUserName = string.IsNullOrEmpty(userName) ? "Usuario" : string.Join("_", userName.Split(Path.GetInvalidFileNameChars()));

                // Nombre dinámico para el archivo
                var fileName = $"ReporteHistorico_{sanitizedUserName}.csv";

                var fileBytes = Encoding.UTF8.GetBytes(csvData.ToString());
                return File(fileBytes, "text/csv", fileName);
            }

            return BadRequest("El reporte no se pudo generar."); // Opcional, manejar el caso en que no se exporta
        }
    }
}
