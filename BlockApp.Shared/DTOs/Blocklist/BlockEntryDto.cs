using BlockApp.Shared.Enums;

namespace BlockApp.Shared.DTOs.Blocklist;

public class BlockEntryDto
{
    /// <summary>Id ของ UserBlockEntry — ใช้สำหรับ DELETE</summary>
    public int UserBlockEntryId { get; set; }

    public int BlockEntryId { get; set; }
    public BlockEntryType EntryType { get; set; }

    // ──── Phone ────
    public string? PhoneNumber { get; set; }

    // ──── Bank account ────
    public string? BankName { get; set; }
    public string? AccountNumber { get; set; }
    public string? AccountHolderName { get; set; }

    /// <summary>โน้ตส่วนตัวของผู้ใช้คนนี้</summary>
    public string? Note { get; set; }

    public BlockReason Reasons { get; set; }
    public string? OtherReason { get; set; }

    /// <summary>จำนวนผู้ใช้ทั้งหมดที่บล็อกรายการนี้</summary>
    public int BlockedByCount { get; set; }

    public DateTime CreatedAt { get; set; }
}
