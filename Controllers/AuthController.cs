using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeVault.Data;
using SafeVault.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SafeVault.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Not a real login, just used to test InputValidation from the WebForm
        [HttpPost("/auth/login")]
        public IActionResult Login(string username, string email)
        {
            if (!InputValidation.IsValid(username) || !InputValidation.IsValid(email))
            {
                return BadRequest("Invalid input");
            }
            // Authentication logic to be added here later
            return Ok("Login successful!");
        }

        [HttpPost("/auth/register/admin")]
        public IActionResult RegisterAdmin([FromBody] UserDto registrationDto)
        {
            return RegisterUser(new UserDto
            {
                Email = registrationDto.Email,
                Password = registrationDto.Password,
                Role = "ADMIN"
            });
        }

        [HttpPost("/auth/register/user")]
        public IActionResult RegisterUser([FromBody] UserDto registrationDto)
        {
            Console.WriteLine($"Registering user: {registrationDto.Email} with role: {registrationDto.Role}");
            if (_context.Users.Any(u => u.Email == registrationDto.Email))
            {
                return BadRequest("Email already exists.");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registrationDto.Password);
            var user = new User { Email = registrationDto.Email, PasswordHash = hashedPassword, Role = registrationDto.Role ?? "USER" };
            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("User registered successfully.");
        }

        [HttpPost("/auth/login/admin")]
        public IActionResult LoginAdmin([FromBody] UserDto loginDto)
        {
            return LoginUser(new UserDto
            {
                Email = loginDto.Email,
                Password = loginDto.Password,
                Role = "ADMIN"
            });
        }

        [HttpPost("/auth/login/user")]
        public IActionResult LoginUser([FromBody] UserDto loginDto)
        {
            // Default the role to "USER" if not provided
            loginDto.Role ??= "USER";

            Console.WriteLine($"Logging in user: {loginDto.Email} with role: {loginDto.Role}");
            var user = _context.Users.FirstOrDefault(u => u.Email == loginDto.Email && u.Role == loginDto.Role);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid credentials.");
            }

            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class UserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // Optional, defaults to "USER"
    }
}
