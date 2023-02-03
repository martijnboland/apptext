using AppText.Features.Application;
using AppText.Features.ContentDefinition;
using AppText.Features.ContentManagement;
using Microsoft.EntityFrameworkCore;

namespace AppText.Storage.EfCore
{
    public class AppTextDbContext: DbContext
    {
        public AppTextDbContext(DbContextOptions<AppTextDbContext> options) : base(options)
        {
        }

        public DbSet<App> Apps { get; set; }
        public DbSet<ApiKey> ApiKeys { get; set; }
        public DbSet<ContentType> ContentTypes { get; set; }
        public DbSet<ContentCollection> ContentCollections { get; set; }
        public DbSet<ContentItem> ContentItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<App>()
                .HasKey(a => a.Id);
            modelBuilder.Entity<App>()
                .Property(a => a.Languages)
                .HasJsonConversion();

            modelBuilder.Entity<ContentType>()
                .HasKey(ct => ct.Id);
            modelBuilder.Entity<ContentType>()
                .Property(ct => ct.ContentFields)
                .HasJsonConversion();
            modelBuilder.Entity<ContentType>()
                .Property(ct => ct.MetaFields)
                .HasJsonConversion();

            modelBuilder.Entity<ContentCollection>()
                .HasKey(ct => ct.Id);
            modelBuilder.Entity<ContentCollection>()
                .Property(cc => cc.ContentType)
                .HasJsonConversion();

            modelBuilder.Entity<ContentItem>()
                .HasKey(ct => ct.Id);
            modelBuilder.Entity<ContentItem>()
                .Property(ci => ci.Content)
                .HasJsonConversion();
            modelBuilder.Entity<ContentItem>()
                .Property(ci => ci.Meta)
                .HasJsonConversion();

            base.OnModelCreating(modelBuilder);
        }
    }
}
