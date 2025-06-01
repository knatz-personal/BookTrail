namespace BookTrail.API.Auth
{
    public class AuthOptions
    {
        public const string ConfigurationSection = "Auth";

        public JwtOptions Jwt { get; set; } = new();
    }
}