using System.Collections.Generic;
using System.Security.Claims;

namespace AppText.Features.User
{
    public class User
    {
        public string Identifier { get; set; }
        public string Name { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
    }
}
