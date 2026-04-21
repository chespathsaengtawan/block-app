
using BlockApp.Shared.Enums;

namespace BlockApp.Shared.DTOs.Auth
{
    public class RequestOtpDto
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public SmsProvider FromService { get; set; } = SmsProvider.ThaibulkSMS;
    }
}