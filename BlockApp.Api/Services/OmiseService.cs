using Omise;
using Omise.Models;
using System.Net.Http.Json;
using System.Text.Json;
using BlockApp.Shared.DTOs.Payment;
using BlockApp.Api.Services.Interfaces;

namespace BlockApp.Api.Services;

public class OmiseService : IOmiseService
{
    private readonly Client _client;
    private readonly ILogger<OmiseService> _logger;

    public OmiseService(IConfiguration configuration, ILogger<OmiseService> logger)
    {
        var publicKey = configuration["Omise:PublicKey"] ?? throw new InvalidOperationException("Omise PublicKey not configured");
        var secretKey = configuration["Omise:SecretKey"] ?? throw new InvalidOperationException("Omise SecretKey not configured");
        
        _client = new Client(publicKey, secretKey);
        _logger = logger;
    }

    public async Task<CreateChargeResult> CreatePromptPayChargeAsync(decimal amount, string description)
    {
        try
        {
            // Step 1: Create a source (PromptPay)
            var source = await _client.Sources.Create(new CreatePaymentSourceRequest
            {
                Amount = (long)(amount * 100), // Convert to satang
                Currency = "thb",
                Type = OffsiteTypes.PromptPay,
                Flow = FlowTypes.Redirect
            });
            
            _logger.LogInformation("Created Omise source: {SourceId}", source.Id);

            // Step 2: Create a charge using the source
            var charge = await _client.Charges.Create(new CreateChargeRequest
            {
                Amount = (long)(amount * 100),
                Currency = "thb",
                Source = source,  // Pass the whole source object
                Description = description,
                ReturnUri = "blockapp://payment/callback"
            });
            
            _logger.LogInformation("Created Omise charge: {ChargeId}", charge.Id);

            return new CreateChargeResult
            {
                ChargeId = charge.Id,
                SourceId = source.Id,
                QrCodeUrl = source.ScannableCode?.Image?.DownloadURI ?? string.Empty,
                Status = charge.Status.ToString(),
                ExpiresAt = source.CreatedAt.AddMinutes(15)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating PromptPay charge");
            throw;
        }
    }

    public async Task<Charge> GetChargeStatusAsync(string chargeId)
    {
        try
        {
            var charge = await _client.Charges.Get(chargeId);
            return charge;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting charge status for {ChargeId}", chargeId);
            throw;
        }
    }
}
