using System.ComponentModel.DataAnnotations;

namespace BookTrail.API.Auth
{
    public class RegisterRequest
    {
        [Required] [EmailAddress] public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required] [Compare(nameof(Password))] public string ConfirmPassword { get; set; } = string.Empty;

        [StringLength(100)] public string FirstName { get; set; }

        [StringLength(100)] public string LastName { get; set; }
    }
}