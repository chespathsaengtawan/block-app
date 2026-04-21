namespace BlockApp.Shared.DTOs.Payment;

public class CreatePaymentDto
{
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = "promptpay"; // promptpay, paynow, truemoney
}

public class PaymentResponseDto
{
    public int PaymentId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? QrCodeUrl { get; set; }
    public string? OmiseChargeId { get; set; }
    public int PointsAmount { get; set; }
    public DateTime ExpiresAt { get; set; }
}

public class CheckPaymentStatusDto
{
    public int PaymentId { get; set; }
}

public class PaymentStatusResponseDto
{
    public string Status { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public int? PointsAdded { get; set; }
    public DateTime? PaidAt { get; set; }
}

public class CreateChargeResult
{
    public string ChargeId { get; set; } = string.Empty;
    public string SourceId { get; set; } = string.Empty;
    public string QrCodeUrl { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
