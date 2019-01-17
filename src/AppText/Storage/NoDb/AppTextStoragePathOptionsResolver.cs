using NoDb;
using System.Threading.Tasks;

namespace AppText.Storage.NoDb
{
    public class AppTextStoragePathOptionsResolver : IStoragePathOptionsResolver
    {
        private readonly string _baseDataPath;

        public AppTextStoragePathOptionsResolver(string baseDataPath)
        {
            _baseDataPath = baseDataPath;
        }

        public Task<StoragePathOptions> Resolve(string projectId)
        {
            var result = new StoragePathOptions();
            result.AppRootFolderPath = _baseDataPath;
            return Task.FromResult(result);
        }
    }
}
