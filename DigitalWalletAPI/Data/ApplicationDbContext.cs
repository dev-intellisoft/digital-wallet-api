using Microsoft.EntityFrameworkCore;
using DigitalWalletAPI.Models;

namespace DigitalWalletAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique email constraint
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // User → Wallet (1:N)
            modelBuilder.Entity<Wallet>()
                .HasOne(w => w.User)
                .WithMany()
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Transaction → Sender Wallet
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Sender)
                .WithMany()
                .HasForeignKey(t => t.SenderWalletId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            // Transaction → Receiver Wallet (nullable)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Receiver)
                .WithMany()
                .HasForeignKey(t => t.ReceiverWalletId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}
