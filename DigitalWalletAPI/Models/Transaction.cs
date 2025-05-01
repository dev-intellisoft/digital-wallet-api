using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalWalletAPI.Models
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // FK: Sender Wallet
        [Required]
        public Guid SenderWalletId { get; set; }

        [ForeignKey(nameof(SenderWalletId))]
        public Wallet Sender { get; set; } = null!;

        // FK: Receiver Wallet (nullable in case of deposit/withdraw)
        public Guid? ReceiverWalletId { get; set; }

        [ForeignKey(nameof(ReceiverWalletId))]
        public Wallet? Receiver { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public string Type { get; set; } = "deposit"; // could be: deposit, withdraw, transfer

        public string? Description { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
