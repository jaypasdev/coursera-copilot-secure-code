using Microsoft.AspNetCore.Mvc;
using SafeVault.Models;

namespace SafeVault.Controllers
{
    public class AuthController : Controller
    {
        [HttpPost]
        public IActionResult Login(string username, string email)
        {
            if (!InputValidation.IsValid(username) || !InputValidation.IsValid(email))
            {
                return BadRequest("Invalid input");
            }
            // Authentication logic to be added here later
            return Ok("Login successful!");
        }
    }
}
