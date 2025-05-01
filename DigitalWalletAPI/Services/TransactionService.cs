using DigitalWalletAPI.Data;
using DigitalWalletAPI.Models;
using DigitalWalletAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DigitalWalletAPI.Services
{
    public class TransactionService
    {
        private readonly ApplicationDbContext _context;

        public TransactionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> GetTransactions(Guid userId)
        {
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (wallet == null) throw new Exception("Carteira não encontrada.");

            return await _context.Transactions
                .Where(t => t.SenderWalletId == wallet.Id || t.ReceiverWalletId == wallet.Id)
                .OrderByDescending(t => t.Timestamp)
                .ToListAsync();
        }

        public async Task<Transaction> AddTransaction(Guid userId, TransactionDto dto, string type)
        {
            if (dto.Amount <= 0)
                throw new ArgumentException("Valor deve ser maior que zero.");

            var sender = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (sender == null)
                throw new Exception("Carteira do remetente não encontrada.");

            Wallet? receiver = null;

            if (type == "transfer")
            {
                if (dto.To == Guid.Empty || dto.To == sender.Id)
                    throw new ArgumentException("ID da carteira de destino inválido.");

                receiver = await _context.Wallets.FirstOrDefaultAsync(w => w.Id == dto.To);
                if (receiver == null)
                    throw new Exception("Carteira de destino não encontrada.");

                if (sender.Balance < dto.Amount)
                    throw new InvalidOperationException("Saldo insuficiente.");

                receiver.Balance += dto.Amount;
                _context.Wallets.Update(receiver);
            }

            var transaction = new Transaction
            {
                SenderWalletId = sender.Id,
                ReceiverWalletId = type == "transfer" ? receiver?.Id : null,
                Amount = dto.Amount,
                Type = type,
                Description = dto.Description,
                Timestamp = DateTime.UtcNow
            };

            sender.Balance += type == "deposit" ? dto.Amount : -dto.Amount;

            _context.Transactions.Add(transaction);
            _context.Wallets.Update(sender);
            await _context.SaveChangesAsync();

            return transaction;
        }
    }
}
