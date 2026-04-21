using BlockApp.Api.Services.Interfaces.Sms;
using Twilio;
using Twilio.Rest.Verify.V2.Service;

namespace BlockApp.Api.Services.Sms
{
    public class TwilioSmsService : ITwilioSmsService
    {
        private readonly string? _serviceSid;
        private readonly bool _configured;

        public TwilioSmsService(IConfiguration config)
        {
            var accountSid = config["Twilio:AccountSid"];
            var authToken  = config["Twilio:AuthToken"];
            _serviceSid    = config["Twilio:VerifyServiceSid"];

            if (!string.IsNullOrWhiteSpace(accountSid) && !string.IsNullOrWhiteSpace(authToken))
            {
                TwilioClient.Init(accountSid, authToken);
                _configured = true;
            }
        }

        public async Task SendOtpAsync(string phoneNumber)
        {
            if (!_configured)
                throw new InvalidOperationException("Twilio is not configured");

            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("PhoneNumber is required");

            await VerificationResource.CreateAsync(
                to: phoneNumber,
                channel: "sms",
                pathServiceSid: _serviceSid
            );
        }

        public async Task<bool> VerifyOtpAsync(string phoneNumber, string code)
        {
            if (!_configured)
                throw new InvalidOperationException("Twilio is not configured");

            if (string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(code))
                return false;

            var check = await VerificationCheckResource.CreateAsync(
                to: phoneNumber,
                code: code,
                pathServiceSid: _serviceSid
            );

            return check.Status == "approved";
        }
    }
}