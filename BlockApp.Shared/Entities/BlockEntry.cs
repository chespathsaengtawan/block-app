using BlockApp.Shared.Enums;

namespace BlockApp.Shared.Entities;

/// <summary>
/// บันทึกเบอร์โทร/เลขบัญชีที่ถูกบล็อก (shared across users)
/// </summary>
public class BlockEntry
{
    public int Id { get; set; }

    /// <summary>ประเภท: เบอร์โทร หรือ เลขบัญชีธนาคาร</summary>
    public BlockEntryType EntryType { get; set; }

    // ──── Phone fields ────
    /// <summary>เบอร์โทรในรูปแบบ E.164 (เช่น +66812345678)</summary>
    public string? PhoneNumber { get; set; }

    // ──── Bank account fields ────
    public string? BankName { get; set; }
    public string? AccountNumber { get; set; }
    public string? AccountHolderName { get; set; }

    /// <summary>ผู้เพิ่มคนแรก</summary>
    public int AddedByUserId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // ──── Navigation ────
    public User AddedBy { get; set; } = null!;
    public ICollection<UserBlockEntry> UserBlockEntries { get; set; } = [];
}
