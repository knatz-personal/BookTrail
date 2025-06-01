namespace BookTrail.API.Auth
{
    public class JwtOptions
    {
        public const string SectionName = "Auth:Jwt";

        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpirationInMinutes { get; set; }
    }
}