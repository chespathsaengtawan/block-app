namespace BlockApp.Shared.Enums;

[Flags]
public enum BlockReason
{
    None        = 0,
    SpamCall    = 1,   // โทรก่อกวน
    Scam        = 2,   // หลอกโอนเงิน
    MuleAccount = 4,   // บัญชีม้า
    Other       = 8    // อื่นๆ (ผู้ใช้กรอกเอง)
}
