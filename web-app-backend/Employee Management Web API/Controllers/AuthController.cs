using Employee_Management_Web_API.Data;
using Employee_Management_Web_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Employee_Management_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly EmployeeContext _employeeContext;

        private readonly IConfiguration _configuration;

        public AuthController(EmployeeContext employeeContext, IConfiguration configuration)
        {
            _employeeContext = employeeContext;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            if (await _employeeContext.Users.AnyAsync(u => u.Email == user.Email))
            {
                return BadRequest(new { message = "Email already exists!" });
            }

            user.Password = HashPassword(user.Password);
            _employeeContext.Users.Add(user);
            await _employeeContext.SaveChangesAsync();

            return Ok(new { message = "User registered successfully!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User loginRequest)
        {
            var user = await _employeeContext.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);
            if (user == null || user.Password != HashPassword(loginRequest.Password))
            {
                return Unauthorized(new { Error = "Invalid email or password" });
            }

            // Generate JWT Token
            var token = GenerateJwtToken(user);
            return Ok(new { loginStatus = true, token });
        }

        private string GenerateJwtToken(User user)
        {
            var secretKey = _configuration["JwtSettings:Secret"]; // Load from config
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                //issuer: "yourdomain.com",
                //audience: "yourdomain.com",
                claims: new[] { new Claim(ClaimTypes.Email, user.Email) },
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        [HttpGet("get-user")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (token == null)
                {
                    return Unauthorized(new { message = "Token is missing!" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]);
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                var email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                if (email == null)
                {
                    return Unauthorized(new { message = "Invalid token!" });
                }

                var user = await _employeeContext.Users
                    .Where(u => u.Email == email)
                    .Select(u => new { u.Id, u.Name, u.Email }) // Exclude password
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return NotFound(new { message = "User not found!" });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error retrieving user", error = ex.Message });
            }
        }

    }

}
