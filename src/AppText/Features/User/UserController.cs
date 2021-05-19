using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
                // Group user claims by claim type and convert to dictionary. Claim values are then serialized to either 
                // a single string or an array of strings when there are multiple claims of the same type.
                var groupedUserClaims = this.User.Claims.GroupBy(c => c.Type).ToDictionary(g => g.Key, g => g.Select(c => c.Value).ToList());
                var claimsDictionary = new Dictionary<string, object>();
                foreach(var userClaimEntry in groupedUserClaims)
                {
                    if (userClaimEntry.Value.Count > 1)
                    {
                        claimsDictionary.Add(userClaimEntry.Key, userClaimEntry.Value);
                    }
                    else
                    {
                        claimsDictionary.Add(userClaimEntry.Key, userClaimEntry.Value.FirstOrDefault());
                    }
                }
                user.Claims = claimsDictionary;
            }
            
            return Ok(user);
        }
    }
}