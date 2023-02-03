using Microsoft.EntityFrameworkCore;

namespace AppText.Storage.EfCore
{
    public static class AppTextDbContextInitializer
    {
        public static void Initialize(this AppTextDbContext context)
        {
            context.Database.Migrate();
        }
    }
}
