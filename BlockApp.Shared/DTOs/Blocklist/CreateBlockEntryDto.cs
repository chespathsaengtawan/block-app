using BlockApp.Shared.Enums;

namespace BlockApp.Shared.DTOs.Blocklist;

public class CreateBlockEntryDto
{
    public BlockEntryType EntryType { get; set; }

    // ──── Phone ────
    /// <summary>เบอร์โทร (รับได้ทั้ง 08x หรือ +668x — server จะแปลงเป็น E.164)</summary>
    public string? PhoneNumber { get; set; }

    // ──── Bank account ────
    public string? BankName { get; set; }
    public string? AccountNumber { get; set; }
    public string? AccountHolderName { get; set; }

    public string? Note { get; set; }

    /// <summary>เหตุผล (Flags: เลือกได้มากกว่า 1)</summary>
    public BlockReason Reasons { get; set; } = BlockReason.None;

    /// <summary>รายละเอียดเพิ่มเติม เมื่อเลือก BlockReason.Other</summary>
    public string? OtherReason { get; set; }
}
