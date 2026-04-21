using System.Net.Http.Json;
using System.Text.Json;
using BlockApp.Shared.DTOs.Payment;
using BlockApp.Shared.DTOs.Points;

namespace BlockApp.App.Services;

public interface IPointsPaymentService
{
    // Points
    Task<decimal> GetBalanceAsync();
    Task TransferPointsAsync(string recipientPhone, decimal amount, string? note);
    Task<List<PointTransactionDto>> GetHistoryAsync(int page = 1);
    Task RecordRewardAsync(string activityType, string? metadata = null);
    Task<Dictionary<string, decimal>> GetRewardRatesAsync();
    
    // Payment
    Task<List<PointsPackageDto>> GetPackagesAsync();
    Task<PaymentResponseDto> CreatePaymentAsync(decimal amount);
    Task<PaymentStatusResponseDto> CheckPaymentStatusAsync(int paymentId);
}

public class PointsPaymentService : IPointsPaymentService
{
    private readonly ApiService _apiService;

    public PointsPaymentService(ApiService apiService)
    {
        _apiService = apiService;
    }

    // Points APIs
    public async Task<decimal> GetBalanceAsync()
    {
        var response = await _apiService.GetAsync<PointsBalanceDto>("blockapp/points/balance");
        return response.Balance;
    }

    public async Task TransferPointsAsync(string recipientPhone, decimal amount, string? note)
    {
        var dto = new TransferPointsDto
        {
            RecipientPhoneNumber = recipientPhone,
            Amount = amount,
            Note = note
        };
        
        await _apiService.PostAsync<object>("blockapp/points/transfer", dto);
    }

    public async Task<List<PointTransactionDto>> GetHistoryAsync(int page = 1)
    {
        return await _apiService.GetAsync<List<PointTransactionDto>>($"blockapp/points/history?page={page}&pageSize=20");
    }

    public async Task RecordRewardAsync(string activityType, string? metadata = null)
    {
        var dto = new RewardActivityDto
        {
            ActivityType = activityType,
            Metadata = metadata
        };
        
        await _apiService.PostAsync<object>("blockapp/points/reward", dto);
    }

    public async Task<Dictionary<string, decimal>> GetRewardRatesAsync()
    {
        return await _apiService.GetAsync<Dictionary<string, decimal>>("blockapp/points/reward-rates");
    }

    // Payment APIs
    public async Task<List<PointsPackageDto>> GetPackagesAsync()
    {
        return await _apiService.GetAsync<List<PointsPackageDto>>("blockapp/payment/packages");
    }

    public async Task<PaymentResponseDto> CreatePaymentAsync(decimal amount)
    {
        var dto = new CreatePaymentDto
        {
            Amount = amount,
            PaymentMethod = "promptpay"
        };
        
        return await _apiService.PostAsync<PaymentResponseDto>("blockapp/payment/create", dto);
    }

    public async Task<PaymentStatusResponseDto> CheckPaymentStatusAsync(int paymentId)
    {
        return await _apiService.GetAsync<PaymentStatusResponseDto>($"blockapp/payment/status/{paymentId}");
    }
}
