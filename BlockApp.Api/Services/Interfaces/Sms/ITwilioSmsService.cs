namespace BlockApp.Api.Services.Interfaces.Sms
{
    public interface ITwilioSmsService
    {
        Task SendOtpAsync(string phoneNumber);
        Task<bool> VerifyOtpAsync(string phoneNumber, string code);
    }
}