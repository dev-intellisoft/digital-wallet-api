using DigitalWalletAPI.Data;
using DigitalWalletAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalWalletAPI.Services
{
    public class WalletService
    {
        private readonly ApplicationDbContext _context;

        public WalletService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Wallet?> GetWalletByUserIdAsync(Guid userId)
        {
            return await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
        }

        public async Task<Wallet> CreateWalletAsync(Guid userId)
        {
            var existingWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (existingWallet != null)
                throw new InvalidOperationException("Carteira já existe.");

            var wallet = new Wallet
            {
                UserId = userId,
                Balance = 0
            };

            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync();

            return wallet;
        }
    }
}
