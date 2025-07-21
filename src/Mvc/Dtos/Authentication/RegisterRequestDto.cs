namespace Mvc.Dtos.Authentication
{
    public class RegisterRequestDto
    {
        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string OrganizationName { get; set; } = null!;
        public string OrganizationEmail { get; set; } = null!;
    }
}
