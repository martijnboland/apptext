using AppText.Core.Shared.Model;
using System.Threading.Tasks;

namespace AppText.Core.Storage
{
    public interface IVersioner
    {
        Task<bool> SetVersion<T>(string appId, T obj) where T : class, IVersionable;
    }
}
