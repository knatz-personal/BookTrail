using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookTrail.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BookTrail.API.Auth
{
    public class AuthService : IAuthService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AuthService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IOptions<JwtOptions> jwtOptions)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            User user = new()
            {
                UserName = registerDto.Username, Email = registerDto.Email, CreatedAt = DateTime.UtcNow
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return new AuthResponseDto
                {
                    Success = false, Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            string token = await GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Success = true,
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationInMinutes)
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            User user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return new AuthResponseDto { Success = false, Message = "Invalid email or password" };
            }

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                return new AuthResponseDto { Success = false, Message = "Invalid email or password" };
            }

            string token = await GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Success = true,
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationInMinutes)
            };
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            IList<Claim> userClaims = await _userManager.GetClaimsAsync(user);
            IList<string> roles = await _userManager.GetRolesAsync(user);

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Add roles as claims
            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Add any additional claims from the user
            claims.AddRange(userClaims);

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}