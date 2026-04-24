namespace BlockApp.App.Models;

public enum HistoryAction
{
    Login,
    Logout,
    BlockAdded,
    BlockAlreadyExisted,
    BlockDeleted,
    SessionRestored
}

public class HistoryEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public HistoryAction Action { get; set; }
    public DateTime OccurredAt { get; set; } = DateTime.Now;

    /// <summary>เบอร์โทร หรือ เลขบัญชีธนาคาร</summary>
    public string? Target { get; set; }

    /// <summary>BankName (สำหรับบัญชีธนาคาร)</summary>
    public string? BankName { get; set; }

    /// <summary>หมายเหตุ / เหตุผลเพิ่มเติม</summary>
    public string? Note { get; set; }
}
