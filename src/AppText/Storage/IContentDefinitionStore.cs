using AppText.Features.ContentDefinition;
using System.Threading.Tasks;

namespace AppText.Storage
{
    public interface IContentDefinitionStore
    {
        Task<ContentType[]> GetContentTypes(ContentTypeQuery query);
        Task<string> AddContentType(ContentType contentType);
        Task UpdateContentType(ContentType contentType);
        Task DeleteContentType(string id, string appId);
    }
}
