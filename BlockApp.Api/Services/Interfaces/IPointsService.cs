using BlockApp.Shared.Entities;
using BlockApp.Shared.DTOs.Points;

namespace BlockApp.Api.Services.Interfaces;

public interface IPointsService
{
    Task<decimal> GetBalanceAsync(int userId);
    Task<PointTransaction> AddPointsAsync(int userId, decimal amount, string description, string? referenceId = null);
    Task<PointTransaction> DeductPointsAsync(int userId, decimal amount, string description);
    Task<PointTransaction> TransferPointsAsync(int fromUserId, string toPhoneNumber, decimal amount, string? note);
    Task<List<PointTransactionDto>> GetTransactionHistoryAsync(int userId, int page = 1, int pageSize = 20);
    Task<RewardActivity> RecordRewardActivityAsync(int userId, string activityType, decimal pointsEarned, string? metadata);
    Task<Dictionary<string, decimal>> GetRewardRatesAsync();
}
