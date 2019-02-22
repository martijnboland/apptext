using System.Collections.Generic;

namespace AppText.Features.User
{
    public class User
    {
        public string Identifier { get; set; }
        public string Name { get; set; }
        public IDictionary<string, string> Claims { get; set; }
    }
}
