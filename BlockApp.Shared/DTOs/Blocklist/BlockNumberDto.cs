namespace BlockApp.Shared.DTOs.Blocklist
{

    public class BlockNumberDto
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}