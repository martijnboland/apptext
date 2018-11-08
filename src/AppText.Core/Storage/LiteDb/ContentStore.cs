using AppText.Core.ContentManagement;
using LiteDB;
using System.Collections.Generic;
using System.Linq;

namespace AppText.Core.Storage.LiteDb
{
    public class ContentStore : IContentStore
    {
        private readonly string _connectionString;

        public ContentStore(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ContentItem[] GetContentItems(ContentItemQuery query)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var queryParams = new List<Query>();
                if (!string.IsNullOrEmpty(query.Id))
                {
                    queryParams.Add(Query.EQ("_id", query.Id));
                }
                if (! string.IsNullOrEmpty(query.CollectionId))
                {
                    queryParams.Add(Query.EQ("CollectionId", query.CollectionId));
                }
                var col = db.GetCollection<ContentItem>();
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

        public string AddContentItem(ContentItem contentItem)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var collection = db.GetCollection<ContentItem>();
                contentItem.Id = ObjectId.NewObjectId().ToString();
                return collection.Insert(contentItem);                
            }
        }

        public void UpdateContentItem(ContentItem contentItem)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var collection = db.GetCollection<ContentItem>();
                collection.Update(contentItem);
            }
        }

        public void DeleteContentItem(string id)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var collection = db.GetCollection<ContentItem>();
                collection.Delete(id);
            }
        }

        public ContentCollection[] GetContentCollections(ContentCollectionQuery query)
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
                var col = db.GetCollection<ContentCollection>();
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

        public string AddContentCollection(ContentCollection contentCollection)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var collection = db.GetCollection<ContentCollection>();
                contentCollection.Id = ObjectId.NewObjectId().ToString();
                return collection.Insert(contentCollection);
            }
        }

        public void UpdateContentCollection(ContentCollection contentCollection)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var collection = db.GetCollection<ContentCollection>();
                collection.Update(contentCollection);
            }
        }

        public void DeleteContentCollection(string id)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var collection = db.GetCollection<ContentCollection>();
                collection.Delete(id);
            }
        }
    }
}
