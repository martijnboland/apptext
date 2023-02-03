using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AppText.Shared.Extensions
{
    public static class IDistributedCacheExtensions
    {
        public static void Set<T>(this IDistributedCache cache,
                                           string cacheKey,
                                           T data,
                                           TimeSpan? absoluteExpireTime = null,
                                           TimeSpan? slidingExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromMinutes(1),
                SlidingExpiration = slidingExpireTime
            };

            var jsonData = JsonConvert.SerializeObject(data);
            cache.SetString(cacheKey, jsonData, options);
        }

        public static T Get<T>(this IDistributedCache cache, string cacheKey)
        {
            var jsonData = cache.GetString(cacheKey);

            if (jsonData is null)
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(jsonData);
        }
    }
}
