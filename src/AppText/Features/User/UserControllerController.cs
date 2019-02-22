using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace AppText.Features.User
{
    [Route("me")]
    [ApiController]
    public class UserControllerController : ControllerBase
    {
        public IActionResult GetCurrentUser()
        {
            var user = new User();
            if (this.User.Identity.IsAuthenticated)
            {
                user.Identifier = this.User.FindFirstValue("sub") ?? this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                user.Name = this.User.Identity.Name;
            }
            else
            {
                user.Name = "anonymous";
            }
            user.Name = this.User.Identity.IsAuthenticated
                ? user.Name = this.User.Identity.Name
                : "anonymous";
            user.Claims = this.User.Claims.ToDictionary(c => c.Type, c => c.Value);
            return Ok(user);
        }
    }
}