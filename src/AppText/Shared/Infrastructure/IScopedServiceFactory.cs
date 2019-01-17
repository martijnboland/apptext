namespace AppText.Shared.Infrastructure
{
    public interface IScopedServiceFactory
    {
        T GetService<T>();
    }
}