using AppText.Core.ContentDefinition;
using System.Threading.Tasks;

namespace AppText.Core.Storage
{
    public interface IContentDefinitionStore
    {
        Task<ContentType[]> GetContentTypes(ContentTypeQuery query);
        Task<string> AddContentType(ContentType contentType);
        Task UpdateContentType(ContentType contentType);
        Task DeleteContentType(string id);
    }
}
