namespace BlockApp.Shared.DTOs.Points;

public class PointsBalanceDto
{
    public decimal Balance { get; set; }
}

public class TransferPointsDto
{
    public string RecipientPhoneNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? Note { get; set; }
}

public class PointTransactionDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? RelatedUserPhone { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class RewardActivityDto
{
    public string ActivityType { get; set; } = string.Empty; // WatchAd, AnswerQuiz, Referral
    public string? Metadata { get; set; }
}

public class PointsPackageDto
{
    public int Points { get; set; }
    public decimal PriceTHB { get; set; }
    public int? BonusPoints { get; set; }
}
