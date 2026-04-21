using System.Text.Json;
using BlockApp.Shared.DTOs.Blocklist;

namespace BlockApp.App.Services;

public class BlocklistCacheService
{
    public const string BlocklistKey = "blocklist_cache_v1";
    public const string NumbersKey = "blocklist_numbers_v1";

    public void SaveBlocklist(List<BlockNumberDto> blocklist)
    {
        try
        {
            Preferences.Default.Set(BlocklistKey, JsonSerializer.Serialize(blocklist));
            var normalized = blocklist.Select(b => Normalize(b.PhoneNumber)).ToList();
            Preferences.Default.Set(NumbersKey, JsonSerializer.Serialize(normalized));
        }
        catch { }
    }

    public List<BlockNumberDto> LoadBlocklist()
    {
        try
        {
            var json = Preferences.Default.Get(BlocklistKey, "[]");
            return JsonSerializer.Deserialize<List<BlockNumberDto>>(json) ?? [];
        }
        catch { return []; }
    }

    public bool IsBlocked(string rawNumber)
    {
        try
        {
            var normalized = Normalize(rawNumber);
            if (string.IsNullOrEmpty(normalized)) return false;
            var json = Preferences.Default.Get(NumbersKey, "[]");
            var numbers = JsonSerializer.Deserialize<List<string>>(json) ?? [];
            return numbers.Contains(normalized);
        }
        catch { return false; }
    }

    /// <summary>
    /// Normalize phone number to Thai local format (0xxxxxxxxx) for comparison.
    /// Strips spaces/dashes, converts +66 international to local format.
    /// </summary>
    public static string Normalize(string number)
    {
        if (string.IsNullOrEmpty(number)) return string.Empty;
        var digits = new string(number.Where(char.IsDigit).ToArray());
        // +66853658387 (11 digits) → 0853658387
        if (digits.StartsWith("66") && digits.Length == 11)
            return "0" + digits[2..];
        return digits;
    }
}
