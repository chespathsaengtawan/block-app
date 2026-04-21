
namespace BlockApp.Shared.Entities
{
    public class BlockNumber
    {


    public int Id { get; set; }

    public int UserId { get; set; }

    public string PhoneNumber { get; set; } = string.Empty;

    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    }
}