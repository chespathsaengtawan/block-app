namespace BlockApp.Shared.Entities;

public class PointsPackage
{
    public int Id { get; set; }
    public int Points { get; set; }
    public decimal PriceTHB { get; set; }
    public int? BonusPoints { get; set; }
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
