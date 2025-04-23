using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using SafeVault.Controllers;
using SafeVault.Data;
using SafeVault.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace SafeVault.Tests
{
    [TestFixture]
    public class TestAuthController
    {
        private ApplicationDbContext _context;
        private AuthController _authController;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Jwt:Key", "YourSuperSecretKeyThatIsAtLeast32Chars" },
                    { "Jwt:Issuer", "SafeVault" },
                    { "Jwt:Audience", "SafeVaultUsers" }
                })
                .Build();

            _authController = new AuthController(_context, configuration);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void TestRegisterUser_Success()
        {
            var userDto = new UserDto
            {
                Email = "user@example.com",
                Password = "UserPassword123",
                Role = "USER"
            };

            var result = _authController.RegisterUser(userDto) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual("User registered successfully.", result.Value);
        }

        [Test]
        public void TestRegisterUser_EmailAlreadyExists()
        {
            _context.Users.Add(new User
            {
                Email = "user@example.com",
                PasswordHash = "hashedpassword",
                Role = "USER"
            });
            _context.SaveChanges();

            var userDto = new UserDto
            {
                Email = "user@example.com",
                Password = "UserPassword123",
                Role = "USER"
            };

            var result = _authController.RegisterUser(userDto) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("Email already exists.", result.Value);
        }

        [Test]
        public void TestLoginUser_Success()
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("UserPassword123");
            _context.Users.Add(new User
            {
                Email = "user@example.com",
                PasswordHash = hashedPassword,
                Role = "USER"
            });
            _context.SaveChanges();

            var loginDto = new UserDto
            {
                Email = "user@example.com",
                Password = "UserPassword123",
                Role = "USER"
            };

            var result = _authController.LoginUser(loginDto) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsTrue(result.Value.ToString().Contains("Token"));
        }

        [Test]
        public void TestLoginUser_InvalidCredentials()
        {
            var loginDto = new UserDto
            {
                Email = "user@example.com",
                Password = "WrongPassword",
                Role = "USER"
            };

            var result = _authController.LoginUser(loginDto) as UnauthorizedObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(401, result.StatusCode);
            Assert.AreEqual("Invalid credentials.", result.Value);
        }
    }
}