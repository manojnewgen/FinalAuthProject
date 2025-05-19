using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FinalAuthProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net; // Explicitly added for clarity

namespace FinalAuthProject.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _signingKey;

        public AuthService(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;

            // Cache the signing key during initialization
            var secret = _configuration["Jwt:Secret"] ??
                throw new ArgumentNullException("Jwt:Secret", "JWT Secret is not configured");
            _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
        }

        public async Task<User?> Authenticate(LoginModel login)
        {
            if (login == null)
                throw new ArgumentNullException(nameof(login));

            var user = await _userManager.FindByNameAsync(login.Username);

            if (user == null || string.IsNullOrEmpty(user.PasswordHash))
                return null;

            // Verify password with enhanced security options
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(
                text: login.Password,
                hash: user.PasswordHash,
                enhancedEntropy: true);

            return isPasswordValid ? user : null;
        }

        public string GenerateJwtToken(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrEmpty(user.Role))
                throw new ArgumentException("User role is required");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(
                    _configuration.GetValue<double>("Jwt:ExpireMinutes", 1440)), // Default 24 hours
                SigningCredentials = new SigningCredentials(
                    _signingKey,
                    SecurityAlgorithms.HmacSha256),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateFakeJwtToken(string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("YourTestSecretKey123!"); // Use a test secret key

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}