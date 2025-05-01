using DigitalWalletAPI.Models.DTOs;
using DigitalWalletAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DigitalWalletAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionService _transactionService;

        public TransactionController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions()
        {
            try
            {
                var userId = GetUserId();
                var transactions = await _transactionService.GetTransactions(userId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit(TransactionDto dto)
        {
            try
            {
                var userId = GetUserId();
                var transaction = await _transactionService.AddTransaction(userId, dto, "deposit");
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer(TransactionDto dto)
        {
            try
            {
                var userId = GetUserId();
                var transaction = await _transactionService.AddTransaction(userId, dto, "transfer");
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        private Guid GetUserId()
        {
            return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }
    }
}
