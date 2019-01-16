namespace AppText.Core.Shared.Infrastructure
{
    public interface IScopedServiceFactory
    {
        T GetService<T>();
    }
}