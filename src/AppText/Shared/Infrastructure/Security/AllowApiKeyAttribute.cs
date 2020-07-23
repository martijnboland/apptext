using System;

namespace AppText.Shared.Infrastructure.Security
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AllowApiKeyAttribute : Attribute
    {
    }
}
