namespace Mvc.Dtos.Authentication
{
    public class RegisterResponseDto
    {
        public string Token { get; set; } = null!;
        public DateTime Expiration { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; } = null!;
        public string OrganizationEmail { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
    }
}
