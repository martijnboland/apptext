using AppText.Core.Storage;
using GraphQL.Types;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AppText.Core.GraphQL
{
    public class SchemaResolver
    {
        private readonly ILogger<SchemaResolver> _logger;
        private readonly Func<IContentStore> _getContentStore;
        private readonly Func<IApplicationStore> _getApplicationStore;
        private readonly IMemoryCache _memoryCache;

        public SchemaResolver(ILogger<SchemaResolver> logger, IMemoryCache memoryCache, Func<IApplicationStore> getApplicationStore, Func<IContentStore> getContentStore)
        {
            _logger = logger;
            _memoryCache = memoryCache;
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

            return await _memoryCache.GetOrCreateAsync(cacheKey, async cacheEntry =>
            {
                return await CreateSchema(appId);
            });
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
