using BlockApp.Shared.Enums;

namespace BlockApp.Shared.Entities;

/// <summary>
/// ความสัมพันธ์ระหว่างผู้ใช้กับรายการบล็อก พร้อมเหตุผลของแต่ละคน
/// </summary>
public class UserBlockEntry
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public int BlockEntryId { get; set; }

    public string? Note { get; set; }

    /// <summary>เหตุผล (Flags: สามารถเลือกได้หลายข้อ)</summary>
    public BlockReason Reasons { get; set; } = BlockReason.None;

    /// <summary>รายละเอียดเพิ่มเติม เมื่อเลือก BlockReason.Other</summary>
    public string? OtherReason { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // ──── Navigation ────
    public User User { get; set; } = null!;
    public BlockEntry BlockEntry { get; set; } = null!;
}
