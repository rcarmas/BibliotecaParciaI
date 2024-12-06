using System;
using System.Linq;
using PersistenceLayer;
using PersistenceLayer.Models;

namespace LibraryManagementSystem.Services.Soap
{
    public class LibraryService : ILibraryService
    {
        private readonly LibraryContext _context;

        public LibraryService(LibraryContext context)
        {
            _context = context;
        }

        // Registrar Préstamo de Libro
        public string RegistrarPrestamo(int userId, string bookId, DateTime borrowDate, DateTime returnDate)
        {
            var book = _context.books.Find(bookId);
            var user = _context.users.Find(userId);

            if (book == null || user == null)
                return "Usuario o libro no encontrado.";

            // Verificar si el libro tiene una transacción activa sin devolver
            var activeTransaction = _context.transactions
                .Where(t => t.bookid == bookId && (t.status == "Prestado" || t.status == "Atrasado"))
                .FirstOrDefault();

            if (activeTransaction != null)
            {
                // Mensaje indicando que el libro está ocupado
                return $"El libro ya está prestado. Fecha esperada de devolución: {activeTransaction.returndate?.ToShortDateString() ?? "No especificada"}";
            }

            // Registrar la nueva transacción de préstamo
            var transaction = new Transaction
            {
                userid = userId,
                bookid = bookId,
                borrowdate = borrowDate.ToUniversalTime(),
                returndate = returnDate.ToUniversalTime(),
                status = "Prestado"
            };

            _context.transactions.Add(transaction);

            // Actualizar el estado del libro a no disponible
            book.availability = false;
            _context.SaveChanges();

            return $"Préstamo registrado. ID de transacción: {transaction.id}";
        }



        // Registrar Devolución de Libro
        public string RegistrarDevolucion(int transactionId)
        {
            var transaction = _context.transactions.Find(transactionId);

            if (transaction == null || transaction.status != "Prestado")
                return "Transacción no válida.";

            var book = _context.books.Find(transaction.bookid);
            transaction.status = "Devuelto";
            transaction.returndate = DateTime.Now.ToUniversalTime();  // Convertir a UTC
            book.availability = true;  // Marcar el libro como disponible

            _context.SaveChanges();

            return "Devolución registrada correctamente.";
        }



        // Generar Reporte de Préstamos Activos
        public string GenerarReportePrestamosActivos(DateTime? startDate, DateTime? endDate)
        {
            // Convertir las fechas a UTC si tienen valor
            if (startDate.HasValue)
                startDate = DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc);

            if (endDate.HasValue)
                endDate = DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc);

            var query = _context.transactions.Where(t => t.status == "Prestado");

            if (startDate.HasValue)
                query = query.Where(t => t.borrowdate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(t => t.borrowdate <= endDate.Value);

            var activeLoans = query
                .Select(t => new
                {
                    t.id,
                    UserName = t.User.name,
                    BookTitle = t.Book.title,
                    borrowDate = t.borrowdate.HasValue ? t.borrowdate.Value.ToShortDateString() : "No disponible",  // Manejo de nulos
                    returnDate = t.returndate.HasValue ? t.returndate.Value.ToShortDateString() : "No devuelto",  // Manejo de nulos
                    t.status
                })
                .ToList();

            return $"Reporte de préstamos activos: {string.Join(", ", activeLoans.Select(l => $"ID: {l.id}, Usuario: {l.UserName}, Libro: {l.BookTitle}, Fecha Préstamo: {l.borrowDate}, Fecha Devolución: {l.returnDate}"))}";
        }

        // Generar Reporte de Historial de Usuario
        public string GenerarReporteHistorialUsuarios(int userId)
        {
            var user = _context.users.Find(userId);
            if (user == null)
                return "Usuario no encontrado.";

            var loanHistory = _context.transactions
                .Where(t => t.userid == userId)
                .Select(t => new
                {
                    BookTitle = t.Book.title,
                    borrowDate = t.borrowdate.HasValue ? t.borrowdate.Value.ToShortDateString() : "No disponible",  // Manejo de nulos
                    returnDate = t.returndate.HasValue ? t.returndate.Value.ToShortDateString() : "No devuelto",  // Manejo de nulos
                    t.status,
                    t.fine
                })
                .ToList();

            return $"Historial de usuario {user.name}: {string.Join(", ", loanHistory.Select(l => $"Libro: {l.BookTitle}, Fecha Préstamo: {l.borrowDate}, Fecha Devolución: {l.returnDate}, Estado: {l.status}, Multa: {l.fine ?? 0}"))}";
        }

        // Registrar Usuario
        public string RegistrarUsuario(string name, string email, string userType)
        {
            var existingUser = _context.users.FirstOrDefault(u => u.email == email);
            if (existingUser != null)
                return "El correo electrónico ya está registrado.";

            var user = new User
            {
                name = name,
                email = email,
                usertype = userType,
                status = "Activo"
            };

            _context.users.Add(user);
            _context.SaveChanges();  // Guardar en la base de datos

            return $"Usuario {name} registrado con éxito. ID de usuario: {user.id}";
        }

        // Suspender Usuario
        public string SuspenderUsuario(int userId)
        {
            var user = _context.users.Find(userId);
            if (user == null)
                return "Usuario no encontrado.";

            user.status = "Suspendido";
            _context.SaveChanges();  // Guardar cambios en la base de datos

            return $"Usuario {user.name} suspendido correctamente.";
        }
    }
}
