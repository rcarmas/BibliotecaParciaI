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

        public string RegisterLoan(int userId, int bookId, DateTime borrowDate, DateTime? returnDate)
        {
            var book = _context.books.Find(bookId);
            var user = _context.users.Find(userId);

            if (book == null || user == null)
                return "Usuario o libro no encontrado.";

            if (!book.availability)
                return "El libro ya está prestado.";

            var transaction = new Transaction
            {
                userid = userId,
                bookid = bookId,
                borrowdate = borrowDate,
                returndate = returnDate,
                status = "Prestado"
            };

            _context.transactions.Add(transaction);
            book.availability = false;  // Marcar el libro como prestado
            _context.SaveChanges();  // Guardar en la base de datos

            return $"Préstamo registrado. ID de transacción: {transaction.id}";
        }

        public string RegisterReturn(int transactionId)
        {
            var transaction = _context.transactions.Find(transactionId);

            if (transaction == null || transaction.status != "Prestado")
                return "Transacción no válida.";

            var book = _context.books.Find(transaction.bookid);
            transaction.status = "Devuelto";
            transaction.returndate = DateTime.Now;  // Fecha actual de devolución
            book.availability = true;  // Marcar el libro como disponible

            // Calcular multas si aplica
            if (transaction.returndate.HasValue && transaction.returndate.Value > transaction.returndate.Value)
            {
                var lateDays = (transaction.returndate.Value - transaction.returndate.Value).TotalDays;
                transaction.fine = (decimal)(lateDays * 1);  // Suponiendo que la multa es 1 por día
            }

            _context.SaveChanges();  // Guardar los cambios en la base de datos

            return "Devolución registrada correctamente.";
        }

    }
}
