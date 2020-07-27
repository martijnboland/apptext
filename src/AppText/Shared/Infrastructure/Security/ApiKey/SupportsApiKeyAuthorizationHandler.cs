using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AppText.Shared.Infrastructure.Security.ApiKey
{
    public class SupportsApiKeyAuthorizationHandler : AuthorizationHandler<SupportsApiKeyRequirement>
    {
        private readonly ILogger<SupportsApiKeyAuthorizationHandler> _logger;

        public SupportsApiKeyAuthorizationHandler(ILogger<SupportsApiKeyAuthorizationHandler> logger)
        {
            _logger = logger;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SupportsApiKeyRequirement requirement)
        {
            return Task.CompletedTask;
        }
    }
}
