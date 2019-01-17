using AppText.Shared.Model;
using LiteDB;
using System.Threading.Tasks;

namespace AppText.Storage.LiteDb
{
    public class Versioner : IVersioner
    {
        private readonly LiteDatabase _liteDatabase;

        public Versioner(LiteDatabase liteDatabase)
        {
            _liteDatabase = liteDatabase;
        }

        public Task<bool> SetVersion<T>(string appId, T obj) where T : class, IVersionable
        {
            if (obj.Id != null)
            {
                // Exisiting object, verify exising version. Throw exception when existing version doesn't match.
                var collection = _liteDatabase.GetCollection<T>();
                if (!collection.Exists(Query.And(Query.EQ("_id", obj.Id), Query.EQ("Version", obj.Version))))
                {
                    return Task.FromResult(false);
                }
            }
            obj.Version++;

            return Task.FromResult(true);
        }
    }
}
