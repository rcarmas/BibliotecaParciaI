using Common.DTOs;
using Microsoft.EntityFrameworkCore;
using PersistenceLayer;
using PersistenceLayer.Models;


namespace BusinessLayer.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly LibraryContext _context;

        public TransactionService(LibraryContext context)
        {
            _context = context;
        }

        public List<TransactionDTO> GetAllTransactions()
        {
            return _context.transactions
                .Select(t => new TransactionDTO
                {
                    Id = t.id,
                    UserName = t.User.name,
                    BookTitle = t.Book.title,
                    BorrowDate = t.borrowdate.Value,
                    ReturnDate = t.returndate,
                    Status = t.status
                }).ToList();
        }

        public List<BookDTO> GetAvailableBooks()
        {
            return _context.books
                .Where(b => b.availability == "")
                .Select(b => new BookDTO
                {
                    Id = b.id,
                    Title = b.title,
                    Author = b.author,
                    Category = b.category,
                    Availability = b.availability,
                    Location = b.location
                }).ToList();
        }
        public string RegisterLoan(Guid userId, string bookId, string status, DateTime borrowDate, DateTime? returnDate, decimal? fine )
        {
            var book = _context.books.Find(bookId);
            var user = _context.users.Find(userId);

            if (book == null || user == null)
                return "Usuario o libro no encontrado.";

            if (book.availability != "Disponible")
                return "Error: El libro no está disponible.";

            var transaction = new Transaction
            {
                userid = userId,  
                bookid = bookId,
                borrowdate = borrowDate.ToUniversalTime(),
                returndate = returnDate?.ToUniversalTime(),
                status = status,
                fine = fine
            };

            _context.transactions.Add(transaction);
            _context.SaveChanges();

            return $"Préstamo registrado. ID de transacción: {transaction.id}";
        }
        public string RegisterReturn(Guid transactionId)
        {
            var transaction = _context.transactions.Find(transactionId);
            if (transaction == null || transaction.status != "Prestado")
                return "Transacción no válida o ya devuelta.";

            transaction.status = "Devuelto";
            transaction.returndate = DateTime.Now.ToUniversalTime();

            _context.SaveChanges();

            return "Devolución registrada correctamente.";
        }
        // Generar reporte de préstamos activos
        public List<TransactionDTO> GenerateActiveLoansReport(DateTime? startDate, DateTime? endDate)
        {
            // Convertir las fechas a UTC si tienen un valor
            DateTime? utcStartDate = startDate?.ToUniversalTime();
            DateTime? utcEndDate = endDate?.ToUniversalTime();

            return _context.transactions.Include(t => t.User).Include(t => t.Book)
                .Where(t =>
                    (!utcStartDate.HasValue || t.borrowdate >= utcStartDate) && // Comparar si está dentro del rango inicial
                    (!utcEndDate.HasValue || t.returndate <= utcEndDate))       // Comparar si está dentro del rango final
                .Select(t => new TransactionDTO
                {
                    Id = t.id,
                    UserId = t.userid,
                    BookId = t.bookid,
                    BorrowDate = t.borrowdate.Value,
                    ReturnDate = t.returndate,
                    Status = t.status,
                    Fine = t.fine,
                    UserName = t.User.name,
                    BookTitle = t.Book.title
                }).ToList();
        }
        // Generar reporte de historial de usuario
        public List<TransactionDTO> GenerateUserHistoryReport(Guid userId)
        {
            return _context.transactions.Include(m=>m.User).Include(m=>m.Book)
                .Where(t => t.userid == userId)
                .Select(t => new TransactionDTO
                {
                    Id = t.id,
                    UserId = t.userid,
                    BookId = t.bookid,
                    BorrowDate = t.borrowdate.Value,
                    ReturnDate = t.returndate,
                    Status = t.status,
                    Fine = t.fine,
                    UserName = t.User.name,
                    BookTitle = t.Book.title
                }).ToList();
        }
        public List<TransactionDTO> GetUserTransactions(Guid userId)
        {
            return _context.transactions
                .Where(t => t.userid == userId)
                .Select(t => new TransactionDTO
                {
                    Id = t.id,
                    BookTitle = t.Book.title,
                    BorrowDate = t.borrowdate.Value,
                    ReturnDate = t.returndate,
                    Status = t.status,
                    Fine = t.fine
                })
                .ToList();
        }
    }
}

