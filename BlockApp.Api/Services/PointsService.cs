using BlockApp.Shared.Entities;
using BlockApp.Shared.DTOs.Points;
using BlockApp.Api.Data;
using BlockApp.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BlockApp.Api.Services;

public class PointsService : IPointsService
{
    private readonly AppDbContext _context;
    private readonly ILogger<PointsService> _logger;

    // Reward rates configuration
    private readonly Dictionary<string, decimal> _rewardRates = new()
    {
        { "WatchAd", 1 },
        { "AnswerQuiz", 5 },
        { "Referral", 50 },
        { "DailyLogin", 2 }
    };

    public PointsService(AppDbContext context, ILogger<PointsService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<decimal> GetBalanceAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        return user?.PointsBalance ?? 0;
    }

    public async Task<PointTransaction> AddPointsAsync(int userId, decimal amount, string description, string? referenceId = null)
    {
        var ownTransaction = _context.Database.CurrentTransaction == null;
        IDbContextTransaction? transaction = ownTransaction
            ? await _context.Database.BeginTransactionAsync()
            : null;

        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found");

            user.PointsBalance += amount;

            var pointTransaction = new PointTransaction
            {
                UserId = userId,
                Amount = amount,
                Type = "Credit",
                Status = "Completed",
                Description = description,
                ReferenceId = referenceId,
                CreatedAt = DateTime.UtcNow,
                CompletedAt = DateTime.UtcNow
            };

            _context.PointTransactions.Add(pointTransaction);
            await _context.SaveChangesAsync();

            if (transaction != null)
                await transaction.CommitAsync();

            _logger.LogInformation("Added {Amount} points to user {UserId}", amount, userId);
            return pointTransaction;
        }
        catch (Exception ex)
        {
            if (transaction != null)
                await transaction.RollbackAsync();
            _logger.LogError(ex, "Error adding points to user {UserId}", userId);
            throw;
        }
        finally
        {
            transaction?.Dispose();
        }
    }

    public async Task<PointTransaction> DeductPointsAsync(int userId, decimal amount, string description)
    {
        var ownTransaction = _context.Database.CurrentTransaction == null;
        IDbContextTransaction? transaction = ownTransaction
            ? await _context.Database.BeginTransactionAsync()
            : null;

        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found");

            if (user.PointsBalance < amount)
                throw new InvalidOperationException("Insufficient points");

            user.PointsBalance -= amount;

            var pointTransaction = new PointTransaction
            {
                UserId = userId,
                Amount = -amount,
                Type = "Deduct",
                Status = "Completed",
                Description = description,
                CreatedAt = DateTime.UtcNow,
                CompletedAt = DateTime.UtcNow
            };

            _context.PointTransactions.Add(pointTransaction);
            await _context.SaveChangesAsync();

            if (transaction != null)
                await transaction.CommitAsync();

            _logger.LogInformation("Deducted {Amount} points from user {UserId}", amount, userId);
            return pointTransaction;
        }
        catch (Exception ex)
        {
            if (transaction != null)
                await transaction.RollbackAsync();
            _logger.LogError(ex, "Error deducting points from user {UserId}", userId);
            throw;
        }
        finally
        {
            transaction?.Dispose();
        }
    }

    public async Task<PointTransaction> TransferPointsAsync(int fromUserId, string toPhoneNumber, decimal amount, string? note)
    {
        var ownTransaction = _context.Database.CurrentTransaction == null;
        IDbContextTransaction? transaction = ownTransaction
            ? await _context.Database.BeginTransactionAsync()
            : null;
        
        try
        {
            var fromUser = await _context.Users.FindAsync(fromUserId);
            var toUser = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == toPhoneNumber);

            if (fromUser == null || toUser == null)
                throw new InvalidOperationException("User not found");

            if (fromUser.PointsBalance < amount)
                throw new InvalidOperationException("Insufficient points");

            if (amount <= 0)
                throw new InvalidOperationException("Transfer amount must be positive");

            // Deduct from sender
            fromUser.PointsBalance -= amount;
            
            // Add to receiver
            toUser.PointsBalance += amount;

            // Record sender transaction
            var senderTransaction = new PointTransaction
            {
                UserId = fromUserId,
                Amount = -amount,
                Type = "Transfer",
                Status = "Completed",
                Description = note ?? $"�͹������ {toPhoneNumber}",
                RelatedUserId = toUser.Id,
                CreatedAt = DateTime.UtcNow,
                CompletedAt = DateTime.UtcNow
            };

            // Record receiver transaction
            var receiverTransaction = new PointTransaction
            {
                UserId = toUser.Id,
                Amount = amount,
                Type = "Transfer",
                Status = "Completed",
                Description = note ?? $"�Ѻ����ҡ {fromUser.PhoneNumber}",
                RelatedUserId = fromUserId,
                CreatedAt = DateTime.UtcNow,
                CompletedAt = DateTime.UtcNow
            };

            _context.PointTransactions.AddRange(senderTransaction, receiverTransaction);
            await _context.SaveChangesAsync();

            if (transaction != null)
                await transaction.CommitAsync();

            _logger.LogInformation("Transferred {Amount} points from user {FromUserId} to {ToUserId}", amount, fromUserId, toUser.Id);
            return senderTransaction;
        }
        catch (Exception ex)
        {
            if (transaction != null)
                await transaction.RollbackAsync();
            _logger.LogError(ex, "Error transferring points");
            throw;
        }
        finally
        {
            transaction?.Dispose();
        }
    }

    public async Task<List<PointTransactionDto>> GetTransactionHistoryAsync(int userId, int page = 1, int pageSize = 20)
    {
        var transactions = await _context.PointTransactions
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(t => t.RelatedUser)
            .Select(t => new PointTransactionDto
            {
                Id = t.Id,
                Amount = t.Amount,
                Type = t.Type,
                Status = t.Status,
                Description = t.Description,
                RelatedUserPhone = t.RelatedUser != null ? t.RelatedUser.PhoneNumber : null,
                CreatedAt = t.CreatedAt
            })
            .ToListAsync();

        return transactions;
    }

    public async Task<RewardActivity> RecordRewardActivityAsync(int userId, string activityType, decimal pointsEarned, string? metadata)
    {
        var ownTransaction = _context.Database.CurrentTransaction == null;
        IDbContextTransaction? transaction = ownTransaction
            ? await _context.Database.BeginTransactionAsync()
            : null;
        
        try
        {
            // Record reward activity
            var activity = new RewardActivity
            {
                UserId = userId,
                ActivityType = activityType,
                PointsEarned = pointsEarned,
                Metadata = metadata,
                CreatedAt = DateTime.UtcNow
            };

            _context.RewardActivities.Add(activity);

            // Add points to user
            await AddPointsAsync(userId, pointsEarned, $"�ҧ��Ũҡ: {activityType}");

            await _context.SaveChangesAsync();

            if (transaction != null)
                await transaction.CommitAsync();

            _logger.LogInformation("Recorded reward activity {ActivityType} for user {UserId}, earned {Points} points", 
                activityType, userId, pointsEarned);
            
            return activity;
        }
        catch (Exception ex)
        {
            if (transaction != null)
                await transaction.RollbackAsync();
            _logger.LogError(ex, "Error recording reward activity");
            throw;
        }
        finally
        {
            transaction?.Dispose();
        }
    }

    public Task<Dictionary<string, decimal>> GetRewardRatesAsync()
    {
        return Task.FromResult(_rewardRates);
    }
}
