using System.ComponentModel.DataAnnotations;

namespace BookTrail.API.Auth
{
    public class RegisterDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Username { get; set; } = string.Empty;

        [Required] [EmailAddress] public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required] [Compare("Password")] public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class LoginDto
    {
        [Required] public string Email { get; set; } = string.Empty;

        [Required] public string Password { get; set; } = string.Empty;
    }

    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? Expiration { get; set; }
        public string Message { get; set; }
    }
}