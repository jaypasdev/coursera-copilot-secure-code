using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SafeVault.Controllers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace SafeVault.Tests
{
    [TestFixture]
    public class TestProtectedController
    {
        private ProtectedController _protectedController;

        [SetUp]
        public void Setup()
        {
            _protectedController = new ProtectedController();
        }

        private void SetUserRole(string role)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, role)
            }, "mock"));

            _protectedController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Test]
        public void TestAccessibleByUserOrAdmin_AsUser()
        {
            SetUserRole("USER");

            var result = _protectedController.AccessibleByUserOrAdmin() as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual("This endpoint is accessible by users with the role USER or ADMIN.", result.Value);
        }

        [Test]
        public void TestAccessibleByUserOrAdmin_AsAdmin()
        {
            SetUserRole("ADMIN");

            var result = _protectedController.AccessibleByUserOrAdmin() as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual("This endpoint is accessible by users with the role USER or ADMIN.", result.Value);
        }

        [Test]
        public void TestAccessibleByAdminOnly_AsAdmin()
        {
            SetUserRole("ADMIN");

            var result = _protectedController.AccessibleByAdminOnly() as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual("This endpoint is accessible only by users with the role ADMIN.", result.Value);
        }

        [Test]
        public void TestAccessibleByAdminOnly_AsUser()
        {
            SetUserRole("USER");

            var result = _protectedController.AccessibleByAdminOnly() as ForbidResult;

            Assert.IsNotNull(result);
        }
    }
}