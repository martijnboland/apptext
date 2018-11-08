using System.Collections.Generic;
using System.Linq;
using AppText.Core.ContentDefinition;
using LiteDB;

namespace AppText.Core.Storage.LiteDb
{
    public class ContentDefinitionStore : IContentDefinitionStore
    {
        private readonly string _connectionString;

        public ContentDefinitionStore(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ContentType[] GetContentTypes(ContentTypeQuery query)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var queryParams = new List<Query>();
                if (!string.IsNullOrEmpty(query.Id))
                {
                    queryParams.Add(Query.EQ("_id", query.Id));
                }
                if (!string.IsNullOrEmpty(query.Name))
                {
                    queryParams.Add(Query.EQ("Name", query.Name));
                }
                var col = db.GetCollection<ContentType>();
                if (queryParams.Count > 1)
                {
                    return col.Find(Query.And(queryParams.ToArray())).ToArray();
                }
                else if (queryParams.Count == 1)
                {
                    return col.Find(queryParams.First()).ToArray();
                }
                else
                {
                    return col.FindAll().ToArray();
                }
            }
        }

        public string AddContentType(ContentType contentType)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var collection = db.GetCollection<ContentType>();
                contentType.Id = ObjectId.NewObjectId().ToString();
                return collection.Insert(contentType);
            }
        }

        public void UpdateContentType(ContentType contentType)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var collection = db.GetCollection<ContentType>();
                collection.Update(contentType);
            }
        }

        public void DeleteContentType(string id)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var collection = db.GetCollection<ContentType>();
                collection.Delete(id);
            }
        }
    }
}
