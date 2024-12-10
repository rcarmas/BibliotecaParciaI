using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Services.Soap;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Data;

namespace LibraryManagementSystem.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILibraryService _libraryService;

        public LoginController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                // Validar si el usuario existe y si la contraseña es correcta
                var user = _libraryService.ValidateUser(email, password);

                if (user == null)
                {
                    ViewBag.Message = "Correo o contraseña incorrectos, o usuario no registrado.";
                    return View("Index"); // Regresar a la vista de login
                }

                // Validar si el usuario tiene estado 'Activo'
                if (user.Status != "Activo")
                {
                    ViewBag.Message = "El usuario no está activo. Por favor, valide su cuenta con el bibliotecario.";
                    return View("Index"); // Regresar a la vista de login
                }

                // Si todo está correcto, iniciar sesión
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Email),
            new Claim("IdUsuario", user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.UserType)
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = true,
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                // Redirigir según el tipo de usuario
                if (user.UserType == "Admin" || user.UserType == "Bibliotecario")
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                else if (user.UserType == "User")
                {
                    return RedirectToAction("Home", "User");
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                ViewBag.Message = "Ocurrió un error al procesar su solicitud. Intente nuevamente.";
            }

            return View("Index");
        }

    }
}