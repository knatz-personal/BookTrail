namespace BookTrail.API.Auth
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}