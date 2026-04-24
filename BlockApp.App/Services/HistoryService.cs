using System.Text.Json;
using BlockApp.App.Models;

namespace BlockApp.App.Services;

public class HistoryService
{
    private const int MaxEntries = 500;
    private static readonly string FilePath =
        Path.Combine(FileSystem.AppDataDirectory, "history_v1.json");

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = false,
        PropertyNameCaseInsensitive = true
    };

    // ─── Read ─────────────────────────────────────────────────

    public List<HistoryEntry> GetAll()
    {
        try
        {
            if (!File.Exists(FilePath)) return [];
            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<HistoryEntry>>(json, _jsonOptions) ?? [];
        }
        catch
        {
            return [];
        }
    }

    // ─── Write ────────────────────────────────────────────────

    public void Log(HistoryAction action, string? target = null, string? bankName = null, string? note = null)
    {
        try
        {
            var entries = GetAll();
            entries.Insert(0, new HistoryEntry
            {
                Action = action,
                Target = target,
                BankName = bankName,
                Note = note
            });

            // Keep only the latest N entries
            if (entries.Count > MaxEntries)
                entries = entries.Take(MaxEntries).ToList();

            File.WriteAllText(FilePath, JsonSerializer.Serialize(entries, _jsonOptions));
        }
        catch { }
    }

    public void Clear()
    {
        try { File.Delete(FilePath); }
        catch { }
    }

    // ─── Display helpers ──────────────────────────────────────

    public static string ActionLabel(HistoryAction action) => action switch
    {
        HistoryAction.Login            => "🔐 เข้าสู่ระบบ",
        HistoryAction.Logout           => "🚪 ออกจากระบบ",
        HistoryAction.BlockAdded       => "🚫 บล็อกสำเร็จ",
        HistoryAction.BlockAlreadyExisted => "➕ เพิ่มบล็อกที่มีอยู่แล้ว",
        HistoryAction.BlockDeleted     => "✅ ยกเลิกการบล็อก",
        HistoryAction.SessionRestored  => "🔄 กู้คืน session",
        _                              => action.ToString()
    };

    public static Color ActionColor(HistoryAction action) => action switch
    {
        HistoryAction.Login            => Color.FromArgb("#7C3AED"),
        HistoryAction.Logout           => Color.FromArgb("#6B7280"),
        HistoryAction.BlockAdded       => Color.FromArgb("#DC2626"),
        HistoryAction.BlockAlreadyExisted => Color.FromArgb("#D97706"),
        HistoryAction.BlockDeleted     => Color.FromArgb("#059669"),
        HistoryAction.SessionRestored  => Color.FromArgb("#2563EB"),
        _                              => Color.FromArgb("#374151")
    };
}
