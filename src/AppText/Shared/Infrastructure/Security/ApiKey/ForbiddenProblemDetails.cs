using Microsoft.AspNetCore.Mvc;

namespace AppText.Shared.Infrastructure.Security.ApiKey
{
    public class ForbiddenProblemDetails : ProblemDetails
    {
        public ForbiddenProblemDetails(string details = null)
        {
            Title = "Forbidden";
            Detail = details;
            Status = 403;
            Type = "https://httpstatuses.com/403";
        }
    }
}
