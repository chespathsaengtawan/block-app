using System.Security.Cryptography;
using System.Text;

namespace BlockApp.App.Services;

public class PinService
{
    private const string PinHashKey = "pin_hash";

    public async Task SavePinAsync(string pin)
    {
        var hash = HashPin(pin);
        await SecureStorage.Default.SetAsync(PinHashKey, hash);
    }

    public async Task<bool> VerifyPinAsync(string pin)
    {
        try
        {
            var stored = await SecureStorage.Default.GetAsync(PinHashKey);
            if (string.IsNullOrEmpty(stored)) return false;
            return stored == HashPin(pin);
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> HasPinAsync()
    {
        try
        {
            var stored = await SecureStorage.Default.GetAsync(PinHashKey);
            return !string.IsNullOrEmpty(stored);
        }
        catch
        {
            return false;
        }
    }

    public Task ClearPinAsync()
    {
        try { SecureStorage.Default.Remove(PinHashKey); } catch { }
        return Task.CompletedTask;
    }

    private static string HashPin(string pin)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(pin));
        return Convert.ToHexString(bytes);
    }
}
