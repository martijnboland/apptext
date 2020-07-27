using System;

namespace AppText.Shared.Infrastructure.Security.ApiKey
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AllowApiKeyAttribute : Attribute
    {
    }
}
