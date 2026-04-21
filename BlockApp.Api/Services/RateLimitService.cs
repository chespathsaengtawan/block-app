using BlockApp.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace BlockApp.Api.Services
{
    public class RateLimitService
    {
        private readonly AppDbContext _db;
    
        public RateLimitService(AppDbContext db)
        {
            _db = db;
        }
    
        public async Task<bool> CanRequestOtpAsync(string phoneNumber)
        {
            var since = DateTime.UtcNow.AddMinutes(-5);
    
            var count = await _db.OtpCodes
                .CountAsync(x => x.PhoneNumber == phoneNumber &&
                                 x.CreatedAt >= since);
    
            return count < 3;
        }
    }
}