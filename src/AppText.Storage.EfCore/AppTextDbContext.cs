using AppText.Features.Application;
using AppText.Features.ContentDefinition;
using AppText.Features.ContentManagement;
using Microsoft.EntityFrameworkCore;

namespace AppText.Storage.EfCore
{
    public class AppTextDbContext: DbContext
    {
        public AppTextDbContext(DbContextOptions<AppTextDbContext> options) : base(options)
        {}

        public DbSet<App> Apps { get; set; }
        public DbSet<ApiKey> ApiKeys { get; set; }
        public DbSet<ContentType> ContentTypes { get; set; }
        public DbSet<ContentCollection> ContentCollections { get; set; }
        public DbSet<ContentItem> ContentItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<App>(b =>
            {
                b.HasKey(a => a.Id);

                b.Property(a => a.DefaultLanguage)
                    .HasMaxLength(2);
                b.Property(a => a.Languages)
                    .HasJsonConversion();
            });

            modelBuilder.Entity<ApiKey>(b =>
            {
                b.HasKey(ak => ak.Id);

                b.Property(ak => ak.AppId)
                    .HasMaxLength(20);
                b.Property(ak => ak.Name)
                    .HasMaxLength(100);
                b.Property(ak => ak.Key)
                    .HasMaxLength(256);

                b.HasOne<App>()
                    .WithMany()
                    .HasForeignKey(ak => ak.AppId)
                        .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<ContentType>(b =>
            {
                b.HasKey(ct => ct.Id);

                b.Property(ct => ct.AppId)
                    .IsRequired(false) // Override attribute to enable global content types that can only be created from code.
                    .HasMaxLength(20);
                b.Property(ct => ct.Name)
                    .HasMaxLength(256);
                b.Property(ct => ct.ContentFields)
                    .HasJsonConversion();
                b.Property(ct => ct.MetaFields)
                    .HasJsonConversion();

                b.HasOne<App>()
                    .WithMany()
                    .HasForeignKey(ct => ct.AppId)
                        .OnDelete(DeleteBehavior.NoAction);
            });


            modelBuilder.Entity<ContentCollection>(b =>
            {
                b.HasKey(cc => cc.Id);

                b.Property(ct => ct.AppId)
                    .IsRequired()
                    .HasMaxLength(20);
                b.Property(cc => cc.Name)
                    .HasMaxLength(256);
                b.Property(cc => cc.ListDisplayField)
                    .HasMaxLength(256);
                b.Property(cc => cc.ContentType)
                    .HasJsonConversion();

                b.HasOne<App>()
                    .WithMany()
                    .HasForeignKey(cc => cc.AppId)
                        .OnDelete(DeleteBehavior.NoAction);

                b.HasIndex(cc => cc.Name);
            });

            modelBuilder.Entity<ContentItem>(b =>
            {
                b.HasKey(ci => ci.Id);

                b.Property(ci => ci.Content)
                    .HasJsonConversion();
                b.Property(ci => ci.Meta)
                    .HasJsonConversion();
                b.Property(ci => ci.CreatedBy)
                    .HasMaxLength(256);
                b.Property(ci => ci.LastModifiedBy)
                    .HasMaxLength(256);

                b.HasOne<App>()
                    .WithMany()
                    .HasForeignKey(ci => ci.AppId)
                        .OnDelete(DeleteBehavior.NoAction);
                b.HasOne<ContentCollection>()
                    .WithMany()
                    .HasForeignKey(ci => ci.CollectionId)
                        .OnDelete(DeleteBehavior.NoAction);

                b.HasIndex(ci => ci.ContentKey);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
