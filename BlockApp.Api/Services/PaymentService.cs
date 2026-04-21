using BlockApp.Shared.Entities;
using BlockApp.Shared.DTOs.Payment;
using BlockApp.Shared.DTOs.Points;
using BlockApp.Api.Data;
using BlockApp.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Omise.Models;

namespace BlockApp.Api.Services;

public class PaymentService : IPaymentService
{
    private readonly AppDbContext _context;
    private readonly IOmiseService _omiseService;
    private readonly IPointsService _pointsService;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(
        AppDbContext context, 
        IOmiseService omiseService, 
        IPointsService pointsService,
        ILogger<PaymentService> logger)
    {
        _context = context;
        _omiseService = omiseService;
        _pointsService = pointsService;
        _logger = logger;
    }

    public async Task<List<PointsPackageDto>> GetPointsPackagesAsync()
    {
        var packages = await _context.PointsPackages
            .Where(p => p.IsActive)
            .OrderBy(p => p.DisplayOrder)
            .Select(p => new PointsPackageDto
            {
                Points = p.Points,
                PriceTHB = p.PriceTHB,
                BonusPoints = p.BonusPoints
            })
            .ToListAsync();

        return packages;
    }

    public int CalculatePointsFromAmount(decimal amount, List<PointsPackageDto> packages)
    {
        var package = packages.FirstOrDefault(p => p.PriceTHB == amount);

        if (package != null)
        {
            return package.Points + (package.BonusPoints ?? 0);
        }

        // Default: 1 THB = 1 Point
        return (int)amount;
    }

    public async Task<PaymentResponseDto> CreatePaymentAsync(int userId, CreatePaymentDto dto)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found");

            // Get packages and calculate points
            var packages = await GetPointsPackagesAsync();
            int pointsAmount = CalculatePointsFromAmount(dto.Amount, packages);

            // Create Omise charge
            var chargeResult = await _omiseService.CreatePromptPayChargeAsync(
                dto.Amount, 
                $"ซื้อแต้ม {pointsAmount} แต้ม");

            // Save payment record
            var payment = new Payment
            {
                UserId = userId,
                Amount = dto.Amount,
                Currency = "THB",
                Status = "Pending",
                OmiseChargeId = chargeResult.ChargeId,
                OmiseSourceId = chargeResult.SourceId,
                PaymentMethod = dto.PaymentMethod,
                QrCodeUrl = chargeResult.QrCodeUrl,
                PointsAmount = pointsAmount,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = chargeResult.ExpiresAt
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created payment {PaymentId} for user {UserId}, amount {Amount} THB", 
                payment.Id, userId, dto.Amount);

            return new PaymentResponseDto
            {
                PaymentId = payment.Id,
                Status = payment.Status,
                QrCodeUrl = payment.QrCodeUrl,
                OmiseChargeId = payment.OmiseChargeId,
                PointsAmount = payment.PointsAmount,
                ExpiresAt = payment.ExpiresAt ?? DateTime.UtcNow.AddMinutes(15)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment for user {UserId}", userId);
            throw;
        }
    }

    public async Task<PaymentStatusResponseDto> CheckPaymentStatusAsync(int paymentId)
    {
        var payment = await _context.Payments.FindAsync(paymentId);
        if (payment == null)
            throw new InvalidOperationException("Payment not found");

        // If already completed, return cached status
        if (payment.Status == "Success")
        {
            return new PaymentStatusResponseDto
            {
                Status = payment.Status,
                IsCompleted = true,
                PointsAdded = payment.PointsAmount,
                PaidAt = payment.PaidAt
            };
        }

        // Check with Omise
        try
        {
            var charge = await _omiseService.GetChargeStatusAsync(payment.OmiseChargeId!);
            
            if (charge.Status.ToString() == "successful" && payment.Status != "Success")
            {
                await ProcessSuccessfulPaymentAsync(payment);
            }
            else if (charge.Status.ToString() == "failed")
            {
                payment.Status = "Failed";
                await _context.SaveChangesAsync();
            }

            return new PaymentStatusResponseDto
            {
                Status = payment.Status,
                IsCompleted = payment.Status == "Success",
                PointsAdded = payment.Status == "Success" ? payment.PointsAmount : null,
                PaidAt = payment.PaidAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking payment status for payment {PaymentId}", paymentId);
            throw;
        }
    }

    public async Task<bool> ProcessWebhookAsync(string chargeId, string status)
    {
        try
        {
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.OmiseChargeId == chargeId);

            if (payment == null)
            {
                _logger.LogWarning("Payment not found for charge {ChargeId}", chargeId);
                return false;
            }

            if (status == "successful" && payment.Status != "Success")
            {
                await ProcessSuccessfulPaymentAsync(payment);
                return true;
            }
            else if (status == "failed")
            {
                payment.Status = "Failed";
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing webhook for charge {ChargeId}", chargeId);
            return false;
        }
    }

    private async Task ProcessSuccessfulPaymentAsync(Payment payment)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            payment.Status = "Success";
            payment.PaidAt = DateTime.UtcNow;

            // Add points to user
            await _pointsService.AddPointsAsync(
                payment.UserId, 
                payment.PointsAmount, 
                $"ซื้อแต้ม {payment.Amount} บาท",
                payment.OmiseChargeId);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Successfully processed payment {PaymentId}, added {Points} points to user {UserId}", 
                payment.Id, payment.PointsAmount, payment.UserId);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error processing successful payment {PaymentId}", payment.Id);
            throw;
        }
    }
}

