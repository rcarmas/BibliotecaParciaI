using Common.DTOs;

namespace BusinessLayer.Services
{
    public interface ITransactionService
    {
        string RegisterLoan(Guid userId, string bookId, string Status, DateTime borrowDate, DateTime? returnDate, decimal? fine);
        string RegisterReturn(Guid transactionId);
        List<TransactionDTO> GenerateActiveLoansReport(DateTime? startDate, DateTime? endDate);
        List<TransactionDTO> GenerateUserHistoryReport(Guid userId);
        List<TransactionDTO> GetUserTransactions(Guid userId);
    }
}
