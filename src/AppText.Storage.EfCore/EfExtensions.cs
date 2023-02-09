using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AppText.Storage.EfCore
{
    public static class EfExtensions
    {
        public static void ForceUpdate<T>(this DbContext dbContext, T entity, Func<T, bool> primaryKeyPredicate) where T : class
        {
            var dbSet = dbContext.Set<T>();

            // Find an existing entry with the same primary key and remove this from the tracker.
            var existingObject = dbSet.Local.FirstOrDefault(primaryKeyPredicate);
            if (existingObject != null)
            {
                dbContext.Entry(existingObject).State = EntityState.Detached;
            }
                
            dbContext.Update(entity);
        }
    }
}
