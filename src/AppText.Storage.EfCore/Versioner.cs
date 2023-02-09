using AppText.Shared.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AppText.Storage.EfCore
{
    public class Versioner : IVersioner
    {
        private readonly AppTextDbContext _dbContext;

        public Versioner(AppTextDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> SetVersion<T>(string appId, T obj) where T : class, IVersionable
        {
            if (obj.Id != null)
            {
                // Exisiting object, verify exising version. Return false when existing version doesn't match.
                var dbSet = _dbContext.Set<T>();
                if (! await dbSet.AnyAsync(x => x.Id == obj.Id && obj.Version == obj.Version))
                {
                    return false;
                }
            }
            obj.Version++;

            return true;
        }
    }
}