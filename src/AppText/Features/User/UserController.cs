using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace AppText.Features.User
{
    [Route("me")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetCurrentUser()
        {
            var user = new User();
            {
                user.Identifier = this.User.Identity.IsAuthenticated ? this.User.FindFirstValue("sub") ?? this.User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
                user.Name = this.User.Identity.IsAuthenticated ? user.Name = this.User.Identity.Name : "Anonymous";
                user.Claims = this.User.Claims;
            }
            
            return Ok(user);
        }
    }
}