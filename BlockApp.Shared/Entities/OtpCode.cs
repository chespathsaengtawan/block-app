using BlockApp.Shared.Enums;
namespace BlockApp.Shared.Entities
{
    public class OtpCode
    {

    public int Id { get; set; }

    public string PhoneNumber { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public DateTime ExpiredAt { get; set; }

    public bool IsUsed { get; set; }

    public OtpPurpose Purpose { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}