using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AppText.Core.Shared.Infrastructure
{
    public class HttpContextScopedServiceFactory : IScopedServiceFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextScopedServiceFactory(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public T GetService<T>()
        {
            return _httpContextAccessor.HttpContext.RequestServices.GetService<T>();
        }
    }
}
