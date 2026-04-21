namespace BlockApp.Api.Services.Interfaces.Sms
{
    public interface IThaibulkSmsService
    {
        /// <summary>
        /// ส่ง OTP ไปยังหมายเลขปลายทาง และคืน token จาก ThaibulkSMS
        /// </summary>
        Task<string> SendOtpAsync(string phoneNumber);

        /// <summary>
        /// ยืนยัน PIN กับ ThaibulkSMS โดยใช้ token ที่ได้รับจาก SendOtpAsync
        /// </summary>
        Task<bool> VerifyOtpAsync(string token, string pin);
    }
}
