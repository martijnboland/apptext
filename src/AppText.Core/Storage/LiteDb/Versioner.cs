using AppText.Core.Shared.Model;
using LiteDB;

namespace AppText.Core.Storage.LiteDb
{
    public class Versioner : IVersioner
    {
        private readonly LiteDatabase _liteDatabase;

        public Versioner(LiteDatabase liteDatabase)
        {
            _liteDatabase = liteDatabase;
        }

        public bool SetVersion<T>(T obj) where T : IVersionable
        {
            if (obj.Id != null)
            {
                // Exisiting object, verify exising version. Throw exception when existing version doesn't match.
                var collection = _liteDatabase.GetCollection<T>();
                if (!collection.Exists(Query.And(Query.EQ("_id", obj.Id), Query.EQ("Version", obj.Version))))
                {
                    return false;
                }
            }
            obj.Version++;

            return true;
        }
    }
}
