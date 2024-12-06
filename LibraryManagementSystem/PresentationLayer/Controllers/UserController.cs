using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Services.Soap;
using Common.DTOs;
using PersistenceLayer.Models;

namespace LibraryManagementSystem.Controllers
{
   
    public class UserController : Controller
    {
        private readonly ILibraryService _libraryService;

        public UserController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
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
            Console.WriteLine($"Nombre: {userDTO.Name}, Email: {userDTO.Email}, Tipo de Usuario: {userDTO.UserType}");
            if (ModelState.IsValid)
            {
                var result = _libraryService.RegistrarUsuario(userDTO.Name, userDTO.Email, userDTO.UserType);
                ViewBag.Message = result;
                return View("CreateUser", userDTO);
            }
            else
            {
                ViewBag.Message = "El formulario contiene errores, por favor verifica los campos.";
                return View("CreateUser", userDTO);
            }
        }


    }
}
