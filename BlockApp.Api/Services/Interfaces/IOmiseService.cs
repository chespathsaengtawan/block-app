using BlockApp.Shared.DTOs.Payment;
using Omise.Models;

namespace BlockApp.Api.Services.Interfaces;

public interface IOmiseService
{
    Task<CreateChargeResult> CreatePromptPayChargeAsync(decimal amount, string description);
    Task<Charge> GetChargeStatusAsync(string chargeId);
}
