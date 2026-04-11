namespace Identity.Application.DTOs
{

    public class AuthResultDto
    {
        public string AccessToken { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }

        public Guid UserId { get; set; }
        public string Email { get; set; } = default!;
    }

}
