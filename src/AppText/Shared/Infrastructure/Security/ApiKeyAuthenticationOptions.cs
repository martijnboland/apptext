using Microsoft.AspNetCore.Authentication;

namespace AppText.Shared.Infrastructure.Security
{
    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "AppText API Key";
        public string Scheme => DefaultScheme;
        public string AuthenticationType = DefaultScheme;
    }
}
