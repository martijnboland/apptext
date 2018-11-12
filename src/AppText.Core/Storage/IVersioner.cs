using AppText.Core.Shared.Model;

namespace AppText.Core.Storage
{
    public interface IVersioner
    {
        bool SetVersion<T>(T obj) where T : IVersionable;
    }
}
