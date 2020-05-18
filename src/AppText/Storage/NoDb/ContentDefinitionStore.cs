using System;
using System.Collections.Generic;
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
            var projectId = contentType.AppId ?? Constants.GlobalProjectId;
            contentType.Id = Guid.NewGuid().ToString();
            await _commands.CreateAsync(projectId, contentType.Id, contentType);
            return contentType.Id;
        }

        public Task DeleteContentType(string id, string appId)
        {
            var projectId = appId ?? Constants.GlobalProjectId;
            return _commands.DeleteAsync(projectId, id);
        }

        public async Task<ContentType[]> GetContentTypes(ContentTypeQuery query)
        {
            var globalContentTypes = (query.AppId == null || query.IncludeGlobalContentTypes)
                ? await _queries.GetAllAsync(Constants.GlobalProjectId)
                : new List<ContentType>();
            var contentTypesForAppId = ! String.IsNullOrEmpty(query.AppId)
                ? await _queries.GetAllAsync(query.AppId)
                : new List<ContentType>();
            var contentTypes = globalContentTypes.Union(contentTypesForAppId);

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
            var projectId = contentType.AppId ?? Constants.GlobalProjectId;
            return _commands.UpdateAsync(projectId, contentType.Id, contentType);
        }
    }
}
