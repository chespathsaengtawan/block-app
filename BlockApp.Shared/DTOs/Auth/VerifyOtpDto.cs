using BlockApp.Shared.Enums;

namespace BlockApp.Shared.DTOs.Auth
{
    public class VerifyOtpDto
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public SmsProvider FromService { get; set; } = SmsProvider.ThaibulkSMS;
        /// <summary>Token จาก ThaibulkSMS (ได้รับจาก /request-otp response)</summary>
        public string? ProviderToken { get; set; }
    }
}