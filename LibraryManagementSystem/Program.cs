using Microsoft.EntityFrameworkCore;
using BusinessLayer.Services;
using PersistenceLayer;
using SoapCore;  // Para SoapCore
using LibraryManagementSystem.Services.Soap; // Asegúrate de importar el namespace adecuado
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la base de datos
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Servicios SOAP
builder.Services.AddSoapCore();  // Para agregar soporte SOAP
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ILibraryService, LibraryService>();  // Registrar el servicio SOAP

// Servicios REST
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

// Agregar controladores (para los servicios REST)
builder.Services.AddControllers();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.Cookie.HttpOnly = true;
        //options.SlidingExpiration = true;
        options.Cookie.Name = "Autorizacion";
        options.AccessDeniedPath = "/Forbidden/";

        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;

        options.Cookie.SameSite = SameSiteMode.Strict;
        options.LoginPath = new PathString("/Identity/Cuenta/Login");
        options.AccessDeniedPath = new PathString("/Identity/Cuenta/SinAutorizacion");
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
    });

builder.Services.ConfigureApplicationCookie(m =>
{
    m.LoginPath = new PathString("/Identity/Cuenta/Login");
    m.AccessDeniedPath = new PathString("/Identity/Cuenta/SinAutorizacion");
});

// Configuración para el enrutamiento de vistas y controladores
var app = builder.Build();

// Habilitar el uso de vistas y controladores MVC
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "user",
    pattern: "{controller=User}/{action=CreateUser}/{id?}");

// Configurar el endpoint SOAP
app.UseSoapEndpoint<ILibraryService>("/LibraryService.svc", new SoapEncoderOptions
{
    // SoapCore maneja la serialización automáticamente sin necesidad de especificar el Serializer.
});

// Habilitar el enrutamiento para las vistas de Book
app.MapControllerRoute(
    name: "book",
    pattern: "Book/{action=Index}/{id?}");

// Mapear los controladores REST
app.MapControllers();

app.Run();