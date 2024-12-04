using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Services;
using Common.DTOs;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        // Registrar préstamo de libro
        [HttpPost("register-loan")]
        public IActionResult RegisterLoan(TransactionDTO transaction)
        {
            var result = _transactionService.RegisterLoan(
                transaction.UserId,
                transaction.BookId,
                transaction.BorrowDate,
                transaction.ReturnDate
            );
            return Ok(result);
        }

        // Registrar devolución de libro
        [HttpPost("register-return")]
        public IActionResult RegisterReturn(int transactionId)
        {
            var result = _transactionService.RegisterReturn(transactionId);
            return Ok(result);
        }
    }
}
