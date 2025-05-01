namespace DigitalWalletAPI.Models.DTOs
{
    public class TransactionDto
    {
        public Guid To { get; set; } //it should be an wallet id
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }
}
