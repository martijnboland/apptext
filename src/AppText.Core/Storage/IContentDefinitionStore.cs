using AppText.Core.ContentDefinition;

namespace AppText.Core.Storage
{
    public interface IContentDefinitionStore
    {
        ContentType[] GetContentTypes(ContentTypeQuery query);
        string AddContentType(ContentType contentType);
        void UpdateContentType(ContentType contentType);
        void DeleteContentType(string id);
    }
}
