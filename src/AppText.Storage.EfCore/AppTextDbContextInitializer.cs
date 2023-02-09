using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RunMethodsSequentially;
using System;
using System.Threading.Tasks;

namespace AppText.Storage.EfCore
{
    public class AppTextDbContextInitializer : IStartupServiceToRunSequentially
    {
        public int OrderNum => 10;

        public async ValueTask ApplyYourChangeAsync(IServiceProvider scopedServices)
        {
            var context = scopedServices.GetRequiredService<AppTextDbContext>();
            await context.Database.MigrateAsync();
        }
    }
}
