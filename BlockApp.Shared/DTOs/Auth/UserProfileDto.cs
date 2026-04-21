namespace BlockApp.Shared.DTOs.Auth
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }
}
