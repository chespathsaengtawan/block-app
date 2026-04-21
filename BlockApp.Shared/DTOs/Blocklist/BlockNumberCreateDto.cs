namespace BlockApp.Shared.DTOs.Blocklist
{

    public class BlockNumberCreateDto
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Note { get; set; }
    }

}