using AppText.Features.ContentDefinition;
using System.Threading.Tasks;

namespace AppText.Storage.EfCore
{
    public class ContentDefinitionStore : IContentDefinitionStore
    {
        public Task<string> AddContentType(ContentType contentType)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteContentType(string id, string appId)
        {
            throw new System.NotImplementedException();
        }

        public Task<ContentType[]> GetContentTypes(ContentTypeQuery query)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateContentType(ContentType contentType)
        {
            throw new System.NotImplementedException();
        }
    }
}