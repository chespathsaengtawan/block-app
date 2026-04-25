using BlockApp.Shared.Entities;
using BlockApp.Shared.DTOs.Payment;
using BlockApp.Shared.DTOs.Points;
using BlockApp.Api.Data;
using BlockApp.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Omise.Models;

namespace BlockApp.Api.Services;

public class PaymentService : IPaymentService
{
    private readonly AppDbContext _context;
    private readonly IOmiseService _omiseService;
    private readonly IPointsService _pointsService;
    private readonly ILogger<PaymentService> _logger;
    private readonly IWebHostEnvironment _env;

    public PaymentService(
        AppDbContext context, 
        IOmiseService omiseService, 
        IPointsService pointsService,
        ILogger<PaymentService> logger,
        IWebHostEnvironment env)
    {
        _context = context;
        _omiseService = omiseService;
        _pointsService = pointsService;
        _logger = logger;
        _env = env;
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
                $"เติม {pointsAmount} แต้ม");

            // Save QR image as static file and get public URL
            string? qrPublicUrl = null;
            if (chargeResult.QrImageBytes != null && chargeResult.QrImageBytes.Length > 0)
            {
                var qrDir = Path.Combine(_env.WebRootPath ?? "wwwroot", "qr");
                Directory.CreateDirectory(qrDir);
                var qrFilename = $"{Guid.NewGuid()}.png";
                var qrFilePath = Path.Combine(qrDir, qrFilename);
                await File.WriteAllBytesAsync(qrFilePath, chargeResult.QrImageBytes);
                qrPublicUrl = $"/qr/{qrFilename}";
                _logger.LogInformation("QR image saved to {Path}", qrFilePath);
            }

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
                QrCodeUrl = qrPublicUrl,
                PointsAmount = pointsAmount,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = chargeResult.ExpiresAt
            };

            Console.WriteLine($"Creating payment for user {payment}");

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created payment {PaymentId} for user {UserId}, amount {Amount} THB", 
                payment.Id, userId, dto.Amount);

            return new PaymentResponseDto
            {
                PaymentId = payment.Id,
                Status = payment.Status,
                OmiseChargeId = payment.OmiseChargeId,
                PointsAmount = payment.PointsAmount,
                QrCodeUrl = qrPublicUrl,
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
            Console.WriteLine($"Charge status for payment {payment.OmiseChargeId}: {charge.Status}");
            Console.WriteLine($"Payment record status: {payment.Status}");

            if (charge.Status.ToString() == "Successful" && payment.Status != "Success")
            {
                Console.WriteLine($"Payment {payment.Id} is successful. Processing points addition.");
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

    public async Task<byte[]?> GetPaymentQrAsync(int paymentId, int userId)
    {
        var payment = await _context.Payments.FindAsync(paymentId);
        if (payment == null || payment.UserId != userId || string.IsNullOrEmpty(payment.QrCodeUrl))
            return null;

        // QrCodeUrl is a relative path like /qr/{guid}.png — read from disk
        var relativePath = payment.QrCodeUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        var filePath = Path.Combine(_env.WebRootPath ?? "wwwroot", relativePath);

        if (!File.Exists(filePath))
            return null;

        return await File.ReadAllBytesAsync(filePath);
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
            
            Console.WriteLine($"Processing successful payment {payment.Id} for user {payment.UserId}, points to add: {payment.PointsAmount}");
            // Add points to user
            await _pointsService.AddPointsAsync(
                payment.UserId, 
                payment.PointsAmount, 
                $"ชำระเงิน {payment.Amount} บาท ด้วย {payment.PaymentMethod}",
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

