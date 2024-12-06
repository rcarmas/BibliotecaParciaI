using Microsoft.EntityFrameworkCore;
using BusinessLayer.Services;
using PersistenceLayer;
using SoapCore;  // Para SoapCore
using LibraryManagementSystem.Services.Soap; // Asegúrate de importar el namespace adecuado

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

// Configuración para el enrutamiento de vistas
var app = builder.Build();

// Habilitar el uso de vistas y controladores MVC
app.UseRouting();

// Configurar el endpoint SOAP
app.UseSoapEndpoint<ILibraryService>("/LibraryService.svc", new SoapEncoderOptions
{
    // SoapCore maneja la serialización automáticamente sin necesidad de especificar el Serializer.
});

// Habilitar el enrutamiento para las vistas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Book}/{action=Index}/{id?}");  // Redirige por defecto al controlador Book y su acción Index

// Mapear los controladores REST
app.MapControllers();

app.Run();
