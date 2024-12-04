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

var app = builder.Build();

// Configuración del endpoint SOAP con SoapCore (no es necesario un SoapEncoderOptions explícito)
app.UseSoapEndpoint<ILibraryService>("/LibraryService.svc", new SoapEncoderOptions
{
    // SoapCore maneja la serialización automáticamente sin necesidad de especificar el Serializer.
});


// Mapear los controladores REST
app.MapControllers();

app.Run();
