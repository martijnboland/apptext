using AppText.Features.ContentDefinition;
using LiteDB;
using System.Linq;
using System.Threading.Tasks;

namespace AppText.Storage.LiteDb
{
    public class ContentDefinitionStore : IContentDefinitionStore
    {
        private readonly LiteRepository _liteRepository;

        public ContentDefinitionStore(LiteDatabase liteDatabase)
        {
            _liteRepository = new LiteRepository(liteDatabase);
        }

        public Task<ContentType[]> GetContentTypes(ContentTypeQuery query)
        {
            var q = _liteRepository.Query<ContentType>();
            if (! string.IsNullOrEmpty(query.AppId))
            {
                q = q.Where(ct => ct.AppId == query.AppId);
            }
            if (!string.IsNullOrEmpty(query.Id))
            {
                q = q.Where(ct => ct.Id == query.Id);
            }
            if (!string.IsNullOrEmpty(query.Name))
            {
                q = q.Where(ct => ct.Name == query.Name);
            }
            var result = q.ToArray().OrderBy(ct => ct.Name).ToArray();
            return Task.FromResult(result);
        }

        public Task<string> AddContentType(ContentType contentType)
        {
            contentType.Id = ObjectId.NewObjectId().ToString();
            _liteRepository.Insert(contentType);
            return Task.FromResult(contentType.Id);
        }

        public Task UpdateContentType(ContentType contentType)
        {
            _liteRepository.Update(contentType);
            return Task.CompletedTask;
        }

        public Task DeleteContentType(string id, string appId)
        {
            _liteRepository.Delete<ContentType>(id);
            return Task.CompletedTask;
        }
    }
}
