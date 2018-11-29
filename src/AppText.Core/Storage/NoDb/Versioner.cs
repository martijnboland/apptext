using System;
using System.Threading.Tasks;
using AppText.Core.Shared.Model;
using NoDb;

namespace AppText.Core.Storage.NoDb
{
    public class Versioner : IVersioner
    {
        private readonly IServiceProvider _services;

        public Versioner(IServiceProvider services)
        {
            _services = services;
        }

        public async Task<bool> SetVersion<T>(string appId, T obj) where T : class, IVersionable
        {
            if (obj.Id != null)
            {
                // Exisiting object, verify exising version. Throw exception when existing version doesn't match.
                var queries = _services.GetService(typeof(IBasicQueries<T>)) as IBasicQueries<T>;
                if (queries != null)
                {
                    var existingObject = await queries.FetchAsync(appId, obj.Id) as IVersionable;
                    if (existingObject.Version != obj.Version)
                    {
                        return false;
                    }
                }
            }
            obj.Version++;

            return true;
        }
    }
}
