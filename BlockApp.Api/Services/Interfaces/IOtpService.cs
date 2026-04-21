using BlockApp.Shared.Entities;
using BlockApp.Shared.Enums;

namespace BlockApp.Api.Services.Interfaces
{
    public interface IOtpService
    {
        /// <summary>Returns provider-specific token (Twilio: empty, ThaibulkSMS: token string)</summary>
        Task<string> RequestOtpAsync(string phoneNumber, SmsProvider provider);
        Task<User> VerifyOtpAsync(string phoneNumber, string code, SmsProvider provider, string? providerToken);
    }
}
