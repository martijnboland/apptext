using AppText.Core.ContentManagement;
using LiteDB;
using System.Collections.Generic;
using System.Linq;

namespace AppText.Core.Storage.LiteDb
{
    public class ContentItemStore : IContentItemStore
    {
        private readonly string _connectionString;

        public ContentItemStore(string connectionString)
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

        public string InsertContentItem(ContentItem contentItem)
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
    }
}
