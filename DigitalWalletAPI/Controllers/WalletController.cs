using DigitalWalletAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DigitalWalletAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly WalletService _walletService;

        public WalletController(WalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWallet()
        {
            var userId = GetUserId();
            var wallet = await _walletService.GetWalletByUserIdAsync(userId);

            if (wallet == null)
                return NotFound("Carteira não encontrada.");

            return Ok(new
            {
                wallet.Id,
                wallet.Balance,
                wallet.CreatedAt
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateWallet()
        {
            var userId = GetUserId();

            try
            {
                var wallet = await _walletService.CreateWalletAsync(userId);
                return Ok(new
                {
                    wallet.Id,
                    wallet.Balance,
                    wallet.CreatedAt
                });
            }
            catch (InvalidOperationException ex)
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
