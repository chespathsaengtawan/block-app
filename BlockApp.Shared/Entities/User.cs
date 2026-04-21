
namespace BlockApp.Shared.Entities
{
    public class User
    {

    public int Id { get; set; }

    public string PhoneNumber { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? LastLoginAt { get; set; }

    public bool IsActive { get; set; } = true;

    // Points System
    public decimal PointsBalance { get; set; } = 0;
    
    // Referral Code
    public string? ReferralCode { get; set; }
    public int? ReferredById { get; set; }

    }
}