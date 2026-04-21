using BlockApp.Shared.Enums;

namespace BlockApp.Shared.DTOs.Auth
{
    public class AuthResultDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiresAt { get; set; }
    }

    public class RequestOtpResultDto
    {
        public string Message { get; set; } = string.Empty;
        public SmsProvider FromService { get; set; }
        /// <summary>Token จาก ThaibulkSMS — ต้องส่งกลับมาใน /verify-otp</summary>
        public string? ProviderToken { get; set; }
    }
}