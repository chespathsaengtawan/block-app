namespace BlockApp.Shared.Entities;

public class RewardActivity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public string ActivityType { get; set; } = string.Empty; // WatchAd, AnswerQuiz, Referral
    public decimal PointsEarned { get; set; }
    public string? Metadata { get; set; } // JSON for additional data
    
    public DateTime CreatedAt { get; set; }
}
