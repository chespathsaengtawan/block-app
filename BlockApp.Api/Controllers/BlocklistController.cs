using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using BlockApp.Shared.DTOs.Blocklist;
using BlockApp.Shared.Entities;
using BlockApp.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BlockApp.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/blocklist")]
    public class BlocklistController : ControllerBase
    {
        private readonly AppDbContext _db;

        public BlocklistController(AppDbContext db)
        {
            _db = db;
        }

        private int CurrentUserId =>
            int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

        [HttpGet]
        public async Task<IEnumerable<BlockNumberDto>> Get()
        {
            return await _db.BlockNumbers
                .Where(x => x.UserId == CurrentUserId)
                .Select(x => new BlockNumberDto
                {
                    Id = x.Id,
                    PhoneNumber = x.PhoneNumber,
                    Note = x.Note,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BlockNumberCreateDto dto)
        {
            var entity = new BlockNumber
            {
                UserId = CurrentUserId,
                PhoneNumber = dto.PhoneNumber,
                Note = dto.Note,
                CreatedAt = DateTime.UtcNow
            };

            _db.BlockNumbers.Add(entity);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _db.BlockNumbers
                .FirstOrDefaultAsync(x => x.Id == id &&
                                          x.UserId == CurrentUserId);

            if (entity == null)
                return NotFound();

            _db.BlockNumbers.Remove(entity);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }

}