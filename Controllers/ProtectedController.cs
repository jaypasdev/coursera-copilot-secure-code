using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SafeVault.Controllers
{
    [ApiController]
    [Route("protected")]
    public class ProtectedController : ControllerBase
    {
        [HttpGet("user-or-admin")]
        [Authorize(Roles = "USER,ADMIN")]
        public IActionResult AccessibleByUserOrAdmin()
        {
            return Ok("This endpoint is accessible by users with the role USER or ADMIN.");
        }

        [HttpGet("admin-only")]
        [Authorize(Roles = "ADMIN")]
        public IActionResult AccessibleByAdminOnly()
        {
            // Explicitly check the user's role
            if (!User.IsInRole("ADMIN"))
            {
                return Forbid();
            }
            return Ok("This endpoint is accessible only by users with the role ADMIN.");
        }
    }
}