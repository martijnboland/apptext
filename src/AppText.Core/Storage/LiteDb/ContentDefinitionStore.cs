using AppText.Core.ContentDefinition;
using LiteDB;

namespace AppText.Core.Storage.LiteDb
{
    public class ContentDefinitionStore : IContentDefinitionStore
    {
        private readonly LiteRepository _liteRepository;

        public ContentDefinitionStore(LiteDatabase liteDatabase)
        {
            _liteRepository = new LiteRepository(liteDatabase);
        }

        public ContentType[] GetContentTypes(ContentTypeQuery query)
        {
            var q = _liteRepository.Query<ContentType>();
            if (! string.IsNullOrEmpty(query.AppPublicId))
            {
                q = q.Where(ct => ct.App.PublicId == query.AppPublicId);
            }
            if (!string.IsNullOrEmpty(query.Id))
            {
                q = q.Where(ct => ct.Id == query.Id);
            }
            if (!string.IsNullOrEmpty(query.Name))
            {
                q = q.Where(ct => ct.Name == query.Name);
            }
            return q.ToArray();
        }

        public string AddContentType(ContentType contentType)
        {
            contentType.Id = ObjectId.NewObjectId().ToString();
            return _liteRepository.Insert(contentType);
        }

        public void UpdateContentType(ContentType contentType)
        {
            _liteRepository.Update(contentType);
        }

        public void DeleteContentType(string id)
        {
            _liteRepository.Delete<ContentType>(id);

        }
    }
}
