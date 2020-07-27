using Microsoft.AspNetCore.Authorization;

namespace AppText.Shared.Infrastructure.Security.ApiKey
{
    public class SupportsApiKeyRequirement : IAuthorizationRequirement
    {
    }
}
