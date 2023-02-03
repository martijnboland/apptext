using AppText.Shared.Extensions;
using AppText.Storage;
using GraphQL.Types;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AppText.Features.GraphQL
{
    public class SchemaResolver
    {
        private readonly ILogger<SchemaResolver> _logger;
        private readonly Func<IContentStore> _getContentStore;
        private readonly Func<IApplicationStore> _getApplicationStore;
        private readonly IDistributedCache _cache;

        public SchemaResolver(ILogger<SchemaResolver> logger, IDistributedCache cache, Func<IApplicationStore> getApplicationStore, Func<IContentStore> getContentStore)
        {
            _logger = logger;
            _cache = cache;
            _getApplicationStore = getApplicationStore;
            _getContentStore = getContentStore;
        }

        /// <summary>
        /// Gets the current GraphQL schema for the given appId. When it doesn't exist, a new schema is created based on the content collections.
        /// </summary>
        /// <returns></returns>
        public async Task<ISchema> Resolve(string appId)
        {
            var cacheKey = $"Schema_{appId}";
            var schema = _cache.Get<ISchema>(cacheKey);
            if (schema == null)
            {
                _logger.LogInformation("GraphQL schema for app {0} is not found in the cache. Creating new instance...", appId);
                schema = await CreateSchema(appId);
            }
            return schema;
        }

        public void Clear(string appId)
        {
            _logger.LogInformation("Clearing GraphQL schema for app {0}", appId);

            var cacheKey = $"Schema_{appId}";
            _cache.Remove(cacheKey);
        }

        private async Task<Schema> CreateSchema(string appId)
        {
            var applicationStore = _getApplicationStore();
            var app = await applicationStore.GetApp(appId);
            if (app == null)
            {
                throw new Exception($"Unable to generate GraphQL Schema for app {appId} because it was not found.");
            }

            var schema = new Schema();
            schema.Query = await AppTextQuery.CreateAsync(app, _getContentStore);

            return schema;
        }
    }
}
