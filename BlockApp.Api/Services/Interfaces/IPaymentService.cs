using BlockApp.Shared.DTOs.Payment;
using BlockApp.Shared.DTOs.Points;

namespace BlockApp.Api.Services.Interfaces;

public interface IPaymentService
{
    Task<PaymentResponseDto> CreatePaymentAsync(int userId, CreatePaymentDto dto);
    Task<PaymentStatusResponseDto> CheckPaymentStatusAsync(int paymentId);
    Task<bool> ProcessWebhookAsync(string chargeId, string status);
    Task<List<PointsPackageDto>> GetPointsPackagesAsync();
    int CalculatePointsFromAmount(decimal amount, List<PointsPackageDto> packages);
}

