namespace BlockApp.Shared.Entities;

public class Payment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "THB";
    public string Status { get; set; } = string.Empty; // Pending, Success, Failed, Expired
    
    // Omise specific
    public string? OmiseChargeId { get; set; }
    public string? OmiseSourceId { get; set; }
    public string? PaymentMethod { get; set; } // promptpay, paynow, truemoney, etc.
    public string? QrCodeUrl { get; set; }
    
    public int PointsAmount { get; set; } // Points to credit
    
    public DateTime CreatedAt { get; set; }
    public DateTime? PaidAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}
