using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Services;
using Common.DTOs;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Listar todos los usuarios
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }

        // Actualizar la información de un usuario
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, UserDTO user)
        {
            var result = _userService.UpdateUser(id, user);  // Ahora result es un string
            if (result == "Usuario no encontrado.")
                return NotFound(result);  // Si el mensaje es "Usuario no encontrado", responde con NotFound

            return Ok(result);  // De lo contrario, responde con el mensaje de éxito
        }



        //Reporte de historial de prestamos

        [HttpGet("{id}/prestamos")]
        public IActionResult GetLoanHistory(int id)
        {
            var history = _userService.GetLoanHistory(id);
            if (history == null || !history.Any())
                return NotFound("No se encontró historial de préstamos para este usuario.");

            return Ok(history);
        }

    }
}
