using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using BlockApp.Shared.DTOs.Blocklist;
using BlockApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BlockApp.Api.Controllers;

[ApiController]
[Authorize]
[Route("blockapp/blocklist")]
public class BlocklistController : ControllerBase
{
    private readonly IBlocklistService _service;

    public BlocklistController(IBlocklistService service)
    {
        _service = service;
    }

    private int CurrentUserId =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub)
            ?? throw new InvalidOperationException("User ID claim not found"));

    [HttpGet]
    public async Task<IEnumerable<BlockEntryDto>> Get()
        => await _service.GetBlocklistAsync(CurrentUserId);

    [HttpPost]
    public async Task<IActionResult> Create(CreateBlockEntryDto dto)
    {
        try
        {
            var result = await _service.AddBlockEntryAsync(CurrentUserId, dto);
            return result.AlreadyExisted ? Ok(result) : CreatedAtAction(nameof(Get), result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>ลบรายการบล็อก — ใช้ UserBlockEntryId (ไม่ใช่ BlockEntryId)</summary>
    [HttpDelete("{userBlockEntryId:int}")]
    public async Task<IActionResult> Delete(int userBlockEntryId)
    {
        var deleted = await _service.DeleteBlockEntryAsync(CurrentUserId, userBlockEntryId);
        return deleted ? NoContent() : NotFound();
    }
}