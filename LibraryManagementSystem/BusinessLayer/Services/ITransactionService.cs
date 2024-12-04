namespace BusinessLayer.Services
{
    public interface ITransactionService
    {
        string RegisterLoan(int userId, int bookId, DateTime borrowDate, DateTime? returnDate);
        string RegisterReturn(int transactionId);
    }
}
