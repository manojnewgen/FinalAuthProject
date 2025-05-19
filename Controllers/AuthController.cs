using FinalAuthProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FinalAuthProject.Services;

namespace FinalAuthProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly JwtService _jwtService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(UserManager<User> userManager, IConfiguration configuration, JwtService jwtService, ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _jwtService = jwtService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Login attempt failed due to invalid model state.");
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                _logger.LogWarning("Invalid login attempt for username: {Username}", model.Username);
                return Unauthorized("Invalid username or password.");
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                _logger.LogWarning("Locked out user attempted login: {Username}", model.Username);
                return Unauthorized("User account is locked.");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateAccessToken(user.UserName, userRoles);

            _logger.LogInformation("User logged in successfully: {Username}", user.UserName);

            return Ok(new
            {
                token,
                expiration = DateTime.Now.AddMinutes(int.Parse(_configuration["Jwt:AccessTokenExpiryMinutes"] ?? "15"))
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Registration attempt failed due to invalid model state.");
                return BadRequest(ModelState);
            }

            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                _logger.LogWarning("Registration attempt failed: User already exists with username: {Username}", model.Username);
                return Conflict("User already exists.");
            }

            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Role = "User" // Ensure Role is set to a non-null value
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                _logger.LogError("Registration failed for username: {Username}. Errors: {Errors}", model.Username, result.Errors);
                return BadRequest(result.Errors);
            }

            _logger.LogInformation("User registered successfully: {Username}", user.UserName);
            return Ok("User registered successfully.");
        }

        [HttpPost("refresh-token")]
        public IActionResult RefreshToken([FromBody] string refreshToken)
        {
            // Validate the refresh token (this should be stored securely, e.g., in a database)
            if (string.IsNullOrEmpty(refreshToken) || !ValidateRefreshToken(refreshToken))
            {
                return Unauthorized("Invalid refresh token.");
            }

            // Generate a new access token
            var username = "exampleUser"; // Retrieve the username associated with the refresh token
            var roles = new List<string> { "User" }; // Retrieve roles associated with the user
            var newAccessToken = _jwtService.GenerateAccessToken(username, roles);

            return Ok(new { accessToken = newAccessToken });
        }

        private bool ValidateRefreshToken(string refreshToken)
        {
            // Implement refresh token validation logic (e.g., check against a database)
            return true; // Placeholder for demonstration
        }
    }
}
