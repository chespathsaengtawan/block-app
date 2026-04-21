using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BlockApp.Api.Data;
using BlockApp.Shared.Entities;

namespace BlockApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // TODO: Add Admin role check
public class PointsPackageController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<PointsPackageController> _logger;

    public PointsPackageController(AppDbContext context, ILogger<PointsPackageController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var packages = await _context.PointsPackages
            .OrderBy(p => p.DisplayOrder)
            .ToListAsync();
        return Ok(packages);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var package = await _context.PointsPackages.FindAsync(id);
        if (package == null)
            return NotFound();
        return Ok(package);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePackageDto dto)
    {
        var package = new PointsPackage
        {
            Points = dto.Points,
            PriceTHB = dto.PriceTHB,
            BonusPoints = dto.BonusPoints,
            IsActive = dto.IsActive,
            DisplayOrder = dto.DisplayOrder,
            CreatedAt = DateTime.UtcNow
        };

        _context.PointsPackages.Add(package);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created points package {PackageId}: {Points} points for {Price} THB", 
            package.Id, package.Points, package.PriceTHB);

        return CreatedAtAction(nameof(GetById), new { id = package.Id }, package);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePackageDto dto)
    {
        var package = await _context.PointsPackages.FindAsync(id);
        if (package == null)
            return NotFound();

        package.Points = dto.Points;
        package.PriceTHB = dto.PriceTHB;
        package.BonusPoints = dto.BonusPoints;
        package.IsActive = dto.IsActive;
        package.DisplayOrder = dto.DisplayOrder;
        package.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated points package {PackageId}", id);

        return Ok(package);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var package = await _context.PointsPackages.FindAsync(id);
        if (package == null)
            return NotFound();

        // Soft delete: just set IsActive to false
        package.IsActive = false;
        package.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Deactivated points package {PackageId}", id);

        return NoContent();
    }

    [HttpPost("{id}/activate")]
    public async Task<IActionResult> Activate(int id)
    {
        var package = await _context.PointsPackages.FindAsync(id);
        if (package == null)
            return NotFound();

        package.IsActive = true;
        package.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Activated points package {PackageId}", id);

        return Ok(package);
    }
}

public record CreatePackageDto(
    int Points,
    decimal PriceTHB,
    int? BonusPoints,
    bool IsActive,
    int DisplayOrder
);

public record UpdatePackageDto(
    int Points,
    decimal PriceTHB,
    int? BonusPoints,
    bool IsActive,
    int DisplayOrder
);
