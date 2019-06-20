using System;
using System.Linq;
using System.Threading.Tasks;
using AppText.Features.ContentDefinition;
using NoDb;

namespace AppText.Storage.NoDb
{
    public class ContentDefinitionStore : IContentDefinitionStore
    {
        private readonly IBasicQueries<ContentType> _queries;
        private readonly IBasicCommands<ContentType> _commands;

        public ContentDefinitionStore(IBasicQueries<ContentType> queries, IBasicCommands<ContentType> commands)
        {
            _queries = queries;
            _commands = commands;
        }

        public async Task<string> AddContentType(ContentType contentType)
        {
            contentType.Id = Guid.NewGuid().ToString();
            await _commands.CreateAsync(contentType.AppId, contentType.Id, contentType);
            return contentType.Id;
        }

        public Task DeleteContentType(string id, string appId)
        {
            return _commands.DeleteAsync(appId, id);
        }

        public async Task<ContentType[]> GetContentTypes(ContentTypeQuery query)
        {
            if (String.IsNullOrEmpty(query.AppId))
            {
                throw new ArgumentException("NoDbStorage always requires the AppId property");
            }
            var contentTypes = await _queries.GetAllAsync(query.AppId);
            if (!string.IsNullOrEmpty(query.Id))
            {
                contentTypes = contentTypes.Where(ct => ct.Id == query.Id);
            }
            if (!string.IsNullOrEmpty(query.Name))
            {
                contentTypes = contentTypes.Where(ct => ct.Name == query.Name);
            }
            return contentTypes.OrderBy(ct => ct.Name).ToArray();
        }

        public Task UpdateContentType(ContentType contentType)
        {
            return _commands.UpdateAsync(contentType.AppId, contentType.Id, contentType);
        }
    }
}
