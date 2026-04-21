using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlockApp.Api.Services.Interfaces;
using BlockApp.Shared.DTOs.Points;
using System.Security.Claims;

namespace BlockApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PointsController : ControllerBase
{
    private readonly IPointsService _pointsService;
    private readonly ILogger<PointsController> _logger;

    public PointsController(IPointsService pointsService, ILogger<PointsController> logger)
    {
        _pointsService = pointsService;
        _logger = logger;
    }

    [HttpGet("balance")]
    public async Task<IActionResult> GetBalance()
    {
        try
        {
            var userId = GetUserId();
            var balance = await _pointsService.GetBalanceAsync(userId);
            return Ok(new PointsBalanceDto { Balance = balance });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting points balance");
            return StatusCode(500, new { Message = "Error getting balance" });
        }
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> TransferPoints([FromBody] TransferPointsDto dto)
    {
        try
        {
            var userId = GetUserId();

            if (dto.Amount <= 0)
            {
                return BadRequest(new { Message = "Transfer amount must be greater than 0" });
            }

            var transaction = await _pointsService.TransferPointsAsync(
                userId, 
                dto.RecipientPhoneNumber, 
                dto.Amount, 
                dto.Note);

            return Ok(new { 
                Message = "Transfer successful", 
                TransactionId = transaction.Id,
                Amount = dto.Amount 
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error transferring points");
            return StatusCode(500, new { Message = "Error transferring points" });
        }
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        try
        {
            var userId = GetUserId();
            var history = await _pointsService.GetTransactionHistoryAsync(userId, page, pageSize);
            return Ok(history);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting transaction history");
            return StatusCode(500, new { Message = "Error getting history" });
        }
    }

    [HttpPost("reward")]
    public async Task<IActionResult> RecordReward([FromBody] RewardActivityDto dto)
    {
        try
        {
            var userId = GetUserId();
            var rates = await _pointsService.GetRewardRatesAsync();

            if (!rates.ContainsKey(dto.ActivityType))
            {
                return BadRequest(new { Message = "Invalid activity type" });
            }

            var pointsEarned = rates[dto.ActivityType];
            
            var activity = await _pointsService.RecordRewardActivityAsync(
                userId, 
                dto.ActivityType, 
                pointsEarned, 
                dto.Metadata);

            return Ok(new { 
                Message = "Reward recorded", 
                PointsEarned = pointsEarned,
                ActivityId = activity.Id 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recording reward");
            return StatusCode(500, new { Message = "Error recording reward" });
        }
    }

    [HttpGet("reward-rates")]
    public async Task<IActionResult> GetRewardRates()
    {
        try
        {
            var rates = await _pointsService.GetRewardRatesAsync();
            return Ok(rates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting reward rates");
            return StatusCode(500, new { Message = "Error getting reward rates" });
        }
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            throw new UnauthorizedAccessException("Invalid user token");
        }
        return userId;
    }
}
