namespace Common.DTOs
{
    public class TransactionDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; } 
        public DateTime BorrowDate { get; set; } 
        public DateTime? ReturnDate { get; set; } 
        public string Status { get; set; }
        public decimal? Fine { get; set; }
    }
}
