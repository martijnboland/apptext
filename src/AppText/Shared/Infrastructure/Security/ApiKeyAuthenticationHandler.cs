using AppText.Features.Application;
using AppText.Storage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AppText.Shared.Infrastructure.Security
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private const string ProblemDetailsContentType = "application/problem+json";
        private const string ApiKeyHeaderName = "X-Api-Key";
        private readonly IApplicationStore _applicationStore;

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<ApiKeyAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IApplicationStore applicationStore) : base(options, logger, encoder, clock)
        {
            _applicationStore = applicationStore;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKeyHeaderValues))
            {
                return AuthenticateResult.NoResult();
            }

            var providedApiKey = apiKeyHeaderValues.FirstOrDefault();

            if (apiKeyHeaderValues.Count == 0 || string.IsNullOrWhiteSpace(providedApiKey))
            {
                return AuthenticateResult.NoResult();
            }

            var appId = Context.GetRouteValue("appId").ToString();
            var hashedApiKey = ApiKey.HashKey(providedApiKey, appId);
            var apiKey = (await _applicationStore.GetApiKeys(new ApiKeysQuery { AppId = appId, Key = hashedApiKey })).FirstOrDefault();
            if (apiKey != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, apiKey.Name)
                };

                var identity = new ClaimsIdentity(claims, Options.AuthenticationType);
                var identities = new List<ClaimsIdentity> { identity };
                var principal = new ClaimsPrincipal(identities);
                var ticket = new AuthenticationTicket(principal, Options.Scheme);

                return AuthenticateResult.Success(ticket);
            }

            return AuthenticateResult.Fail("Invalid API Key provided.");
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 401;
            Response.ContentType = ProblemDetailsContentType;
            var problemDetails = new UnauthorizedProblemDetails();

            await Response.WriteAsync(JsonConvert.SerializeObject(problemDetails));
        }

        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 403;
            Response.ContentType = ProblemDetailsContentType;
            var problemDetails = new ForbiddenProblemDetails();

            await Response.WriteAsync(JsonConvert.SerializeObject(problemDetails));
        }
    }
}
