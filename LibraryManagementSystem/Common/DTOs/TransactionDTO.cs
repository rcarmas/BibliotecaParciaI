namespace Common.DTOs
{
    public class TransactionDTO
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string? BookId { get; set; } 
        public DateTime BorrowDate { get; set; } 
        public DateTime? ReturnDate { get; set; } 
        public string Status { get; set; }
        public decimal? Fine { get; set; }
        public string UserName { get; set; }  
        public string BookTitle { get; set; }
    }
}
