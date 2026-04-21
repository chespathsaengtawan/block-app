using BlockApp.Api.Services.Interfaces;
using BlockApp.Api.Data;
using BlockApp.Shared.Entities;
using BlockApp.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using BlockApp.Api.Services.Interfaces.Sms;

namespace BlockApp.Api.Services
{
    public class OtpService : IOtpService
    {
        private readonly AppDbContext _db;
        private readonly RateLimitService _rateLimit;
        private readonly ITwilioSmsService _twilio;
        private readonly IThaibulkSmsService _thaibulk;

        public OtpService(
            AppDbContext db,
            RateLimitService rateLimit,
            ITwilioSmsService twilio,
            IThaibulkSmsService thaibulk)
        {
            _db = db;
            _rateLimit = rateLimit;
            _twilio = twilio;
            _thaibulk = thaibulk;
        }

        public async Task<string> RequestOtpAsync(string phoneNumber, SmsProvider provider)
        {
            if (!await _rateLimit.CanRequestOtpAsync(phoneNumber))
                throw new Exception("Too many OTP requests. Please try again later.");

            if (provider == SmsProvider.ThaibulkSMS)
            {
                var token = await _thaibulk.SendOtpAsync(phoneNumber);
                return token;
            }

            // Twilio Verify — token ไม่จำเป็น (Twilio จัดการเอง)
            await _twilio.SendOtpAsync(phoneNumber);
            return string.Empty;
        }

        public async Task<User> VerifyOtpAsync(string phoneNumber, string code, SmsProvider provider, string? providerToken)
        {
            bool approved;

            if (provider == SmsProvider.ThaibulkSMS)
            {
                if (string.IsNullOrWhiteSpace(providerToken))
                    throw new Exception("ProviderToken is required for ThaibulkSMS verification");

                approved = await _thaibulk.VerifyOtpAsync(providerToken, code);
            }
            else
            {
                approved = await _twilio.VerifyOtpAsync(phoneNumber, code);
            }

            if (!approved)
                throw new Exception("OTP ไม่ถูกต้องหรือหมดอายุ");

            var user = await _db.Users
                .FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);

            if (user == null)
            {
                user = new User
                {
                    PhoneNumber = phoneNumber,
                    CreatedAt = DateTime.UtcNow
                };
                _db.Users.Add(user);
            }

            user.LastLoginAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return user;
        }
    }
}