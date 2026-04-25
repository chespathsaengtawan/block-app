using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using BlockApp.Shared.DTOs.Auth;
using BlockApp.Shared.DTOs.Blocklist;
using BlockApp.Shared.Enums;

namespace BlockApp.App.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    // Physical device: ใช้ IP จริงของ PC (ต้องอยู่ Wi-Fi เดียวกัน)
    // Emulator: 10.0.2.2 → host machine localhost
#if ANDROID
    private const string BaseUrlConst = "http://10.156.89.22:5073";
#else
    private const string BaseUrlConst = "http://localhost:5073";
#endif
    public string BaseUrl => BaseUrlConst;

    public string? AccessToken { get; private set; }

    private readonly BlocklistCacheService _blocklistCache;

    public ApiService(BlocklistCacheService blocklistCache)
    {
        _blocklistCache = blocklistCache;
        var handler = new HttpClientHandler
        {
            // Accept self-signed certificate in development
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
        };

        _httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri(BaseUrl),
            Timeout = TimeSpan.FromSeconds(30)
        };
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public async Task<(RequestOtpResultDto? Result, string? Error)> RequestOtpAsync(string phoneNumber)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                "/blockapp/auth/request-otp",
                new RequestOtpDto { PhoneNumber = phoneNumber, FromService = SmsProvider.ThaibulkSMS });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<RequestOtpResultDto>(_jsonOptions);
                return (result, null);
            }

            var errorBody = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"[ApiService] RequestOtp failed: {(int)response.StatusCode} {errorBody}");
            return (null, $"[{(int)response.StatusCode}] {errorBody}");
        }
        catch (HttpRequestException ex)
        {
            System.Diagnostics.Debug.WriteLine($"[ApiService] RequestOtp network error: {ex.Message}");
            return (null, $"เชื่อมต่อ {BaseUrl} ไม่ได้: {ex.Message}");
        }
        catch (TaskCanceledException)
        {
            return (null, "Request timeout — API ไม่ตอบสนองใน 30 วินาที");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[ApiService] RequestOtp exception: {ex.GetType().Name}: {ex.Message}");
            return (null, $"{ex.GetType().Name}: {ex.Message}");
        }
    }

    public async Task<AuthResultDto?> VerifyOtpAsync(string phoneNumber, string code,
        SmsProvider fromService = SmsProvider.ThaibulkSMS, string? providerToken = null)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                "/blockapp/auth/verify-otp",
                new VerifyOtpDto
                {
                    PhoneNumber = phoneNumber,
                    Code = code,
                    FromService = fromService,
                    ProviderToken = providerToken
                });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResultDto>(_jsonOptions);
                if (result?.AccessToken != null)
                    await SaveTokensAsync(result);
                return result;
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<BlockEntryDto>> GetBlocklistAsync()
    {
        try
        {
            var list = await _httpClient.GetFromJsonAsync<List<BlockEntryDto>>("/blockapp/blocklist", _jsonOptions) ?? [];
            _blocklistCache.SaveBlocklist(list);
            return list;
        }
        catch
        {
            return _blocklistCache.LoadBlocklist();
        }
    }

    public async Task SyncBlocklistAsync()
    {
        try
        {
            var list = await _httpClient.GetFromJsonAsync<List<BlockEntryDto>>("/blockapp/blocklist", _jsonOptions) ?? [];
            _blocklistCache.SaveBlocklist(list);
        }
        catch { }
    }

    public async Task<AddBlockEntryResultDto?> AddBlockEntryAsync(CreateBlockEntryDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/blockapp/blocklist", dto, _jsonOptions);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<AddBlockEntryResultDto>(_jsonOptions);
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> DeleteBlockEntryAsync(int userBlockEntryId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"/blockapp/blocklist/{userBlockEntryId}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public void SetAuthHeader(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<bool> CheckPhoneAsync(string phoneNumber)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                "/blockapp/auth/check-phone",
                new CheckPhoneDto { PhoneNumber = phoneNumber });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<CheckPhoneResultDto>();
                return result?.Exists ?? false;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> RefreshTokenAsync()
    {
        try
        {
            var refreshToken = await SecureStorage.Default.GetAsync("refresh_token");
            if (string.IsNullOrEmpty(refreshToken)) return false;

            var response = await _httpClient.PostAsJsonAsync(
                "/blockapp/auth/refresh",
                new RefreshTokenDto { RefreshToken = refreshToken });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResultDto>(_jsonOptions);
                if (result?.AccessToken != null)
                {
                    await SaveTokensAsync(result);
                    return true;
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task TryRestoreSessionAsync()
    {
        try
        {
            var token = await SecureStorage.Default.GetAsync("access_token");
            if (!string.IsNullOrEmpty(token))
            {
                AccessToken = token;
                SetAuthHeader(token);
            }
        }
        catch { }
    }

    public async Task<bool> IsAccessTokenValidAsync()
    {
        try
        {
            var expiry = await SecureStorage.Default.GetAsync("access_token_expiry");
            if (string.IsNullOrEmpty(expiry)) return false;
            return DateTime.TryParse(expiry, null, System.Globalization.DateTimeStyles.RoundtripKind, out var dt)
                   && dt > DateTime.UtcNow.AddMinutes(1);
        }
        catch { return false; }
    }

    public async Task ClearSessionAsync()
    {
        try
        {
            SecureStorage.Default.Remove("access_token");
            SecureStorage.Default.Remove("refresh_token");
            SecureStorage.Default.Remove("access_token_expiry");
            SecureStorage.Default.Remove("refresh_token_expiry");
            AccessToken = null;
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
        catch { }
        await Task.CompletedTask;
    }

    private async Task SaveTokensAsync(AuthResultDto result)
    {
        AccessToken = result.AccessToken;
        SetAuthHeader(result.AccessToken);
        await SecureStorage.Default.SetAsync("access_token", result.AccessToken);
        await SecureStorage.Default.SetAsync("refresh_token", result.RefreshToken);
        await SecureStorage.Default.SetAsync("access_token_expiry", result.ExpiresAt.ToString("O"));
        await SecureStorage.Default.SetAsync("refresh_token_expiry", result.RefreshTokenExpiresAt.ToString("O"));
    }

    // Generic HTTP methods
    public async Task<T> GetAsync<T>(string endpoint)
    {
        var response = await _httpClient.GetAsync($"/blockapp/{endpoint}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>(_jsonOptions) 
            ?? throw new Exception("Failed to deserialize response");
    }

    public async Task<byte[]> GetBytesAsync(string endpoint)
    {
        var response = await _httpClient.GetAsync($"/blockapp/{endpoint}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsByteArrayAsync();
    }

    public async Task<T> PostAsync<T>(string endpoint, object data)
    {
        var response = await _httpClient.PostAsJsonAsync($"/blockapp/{endpoint}", data, _jsonOptions);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>(_jsonOptions)
            ?? throw new Exception("Failed to deserialize response");
    }

    public async Task DeleteAsync(string endpoint)
    {
        var response = await _httpClient.DeleteAsync($"/blockapp/{endpoint}");
        response.EnsureSuccessStatusCode();
    }
}
