using BlockApp.Api.Services.Interfaces.Sms;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BlockApp.Api.Services.Sms
{
    public class ThaibulkSmsService : IThaibulkSmsService
    {
        private readonly string _appKey;
        private readonly string _appSecret;
        private readonly string _requestUrl;
        private readonly string _verifyUrl;
        private readonly HttpClient _http;

        public ThaibulkSmsService(IConfiguration config, IHttpClientFactory httpFactory)
        {
            _appKey    = config["ThaibulkSMS:AppKey"]    ?? throw new ArgumentNullException("ThaibulkSMS:AppKey");
            _appSecret = config["ThaibulkSMS:AppSecret"] ?? throw new ArgumentNullException("ThaibulkSMS:AppSecret");

            var baseUrl = (config["ThaibulkSMS:BaseUrl"] ?? "https://otp.thaibulksms.com/v2/otp").TrimEnd('/');
            _requestUrl = $"{baseUrl}/request";
            _verifyUrl  = $"{baseUrl}/verify";

            _http = httpFactory.CreateClient("ThaibulkSMS");
        }

        public async Task<string> SendOtpAsync(string phoneNumber)
        {
            var form = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("key",    _appKey),
                new KeyValuePair<string, string>("secret", _appSecret),
                new KeyValuePair<string, string>("msisdn", phoneNumber),
            });

            var response = await _http.PostAsync(_requestUrl, form);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"ThaibulkSMS request-otp failed: {body}");

            using var doc = JsonDocument.Parse(body);
            var root = doc.RootElement;

            var status = root.GetProperty("status").GetString();
            if (status != "success")
                throw new Exception($"ThaibulkSMS request-otp error: {body}");

            return root.GetProperty("token").GetString()
                ?? throw new Exception("ThaibulkSMS returned no token");
        }

        public async Task<bool> VerifyOtpAsync(string token, string pin)
        {
            var form = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("key",    _appKey),
                new KeyValuePair<string, string>("secret", _appSecret),
                new KeyValuePair<string, string>("token",  token),
                new KeyValuePair<string, string>("pin",    pin),
            });

            var response = await _http.PostAsync(_verifyUrl, form);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return false;

            using var doc = JsonDocument.Parse(body);
            var status = doc.RootElement.GetProperty("status").GetString();
            return status == "success";
        }
    }
}
