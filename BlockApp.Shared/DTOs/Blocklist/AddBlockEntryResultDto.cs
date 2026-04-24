namespace BlockApp.Shared.DTOs.Blocklist;

public class AddBlockEntryResultDto
{
    public BlockEntryDto Entry { get; set; } = null!;

    /// <summary>true = มีผู้ใช้อื่นบล็อกเบอร์/บัญชีนี้ไว้แล้ว — เพิ่มให้อัตโนมัติ</summary>
    public bool AlreadyExisted { get; set; }

    /// <summary>จำนวนผู้ใช้ทั้งหมดที่บล็อกรายการนี้ (รวมคนปัจจุบัน)</summary>
    public int BlockedByCount { get; set; }
}
