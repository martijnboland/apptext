using AppText.Shared.Model;
using System.Threading.Tasks;

namespace AppText.Storage
{
    public interface IVersioner
    {
        Task<bool> SetVersion<T>(string appId, T obj) where T : class, IVersionable;
    }
}
