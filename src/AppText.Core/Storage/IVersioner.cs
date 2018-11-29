using AppText.Core.Shared.Model;
using System.Threading.Tasks;

namespace AppText.Core.Storage
{
    public interface IVersioner
    {
        Task<bool> SetVersion<T>(T obj) where T : IVersionable;
    }
}
