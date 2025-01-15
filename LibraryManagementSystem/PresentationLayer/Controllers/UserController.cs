using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Services.Soap;
using Common.DTOs;
using PersistenceLayer.Models;
using System.Security.Claims;
using BusinessLayer.Services;

namespace LibraryManagementSystem.Controllers
{
   
    public class UserController : Controller
    {
        private readonly ILibraryService _libraryService;
        private readonly IUserService _userService;
        private readonly ITransactionService _transactionService;

        public UserController(ILibraryService libraryService, IUserService userService, ITransactionService transactionService)
        {
            _libraryService = libraryService;
            _userService = userService;
            _transactionService = transactionService;
        }
        public IActionResult Home()
        {
            var userIdClaim = User.FindFirst("IdUsuario")?.Value;
            Guid guid = Guid.Parse(userIdClaim);
            var transactions = _transactionService.GetUserTransactions(guid);

            return View(transactions);
        }

        // Vista para crear un nuevo usuario
        public IActionResult CreateUser()
        {
           
            var userDTO = new UserDTO();
            return View(userDTO);
        }

        // Acción para registrar un nuevo usuario
        [HttpPost]
        public IActionResult RegisterUser(UserDTO userDTO)
        {
            Console.WriteLine($"Nombre: {userDTO.Name}, Email: {userDTO.Email}, Estado: {userDTO.Status} ,Tipo de Usuario: {userDTO.UserType}, Contraseña: {userDTO.Password}");
            if (ModelState.IsValid)
            {
                var result = _libraryService.RegistrarUsuario(userDTO.Name, userDTO.Email, userDTO.Status, userDTO.UserType, userDTO.Password);
                ViewBag.Message = result;
                return RedirectToAction("Index", "Login"); ;
            }
            else
            {
                ViewBag.Message = "El formulario contiene errores, por favor verifica los campos.";
                return View("CreateUser", userDTO);
            }
        }

    }
}
