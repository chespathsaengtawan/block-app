namespace BlockApp.Shared.Entities;

public class PointTransaction
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public decimal Amount { get; set; }
    public string Type { get; set; } = string.Empty; // Purchase, Transfer, Reward, Deduct
    public string Status { get; set; } = string.Empty; // Pending, Completed, Failed, Cancelled
    public string? Description { get; set; }
    public string? ReferenceId { get; set; } // Payment ID or Transfer ID
    
    public int? RelatedUserId { get; set; } // For transfers
    public User? RelatedUser { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
