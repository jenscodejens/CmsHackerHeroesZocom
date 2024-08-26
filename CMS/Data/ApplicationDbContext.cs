using CMS.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Content> Contents { get; set; }
        public DbSet<WebPage> WebPages { get; set; }
        public DbSet<WebSite> Websites { get; set; }
        public DbSet<ContentType> ContentTypes { get; set; }
        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WebSite>(e =>
            {
                e.HasKey(e => e.WebSiteId);
                e.HasOne(e => e.ApplicationUser)
                  .WithMany(u => u.WebSites)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<WebPage>(p =>
            {
                p.HasKey(p => p.WebPageId);
                p.HasOne(p => p.WebSites)
                  .WithMany(w => w.WebPages)
                  .HasForeignKey(p => p.WebSiteId)
                  .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<Content>(c =>
            {
                c.HasKey(c => c.ContentId);
                c.HasOne(c => c.WebPages)
                    .WithMany(w => w.Contents)
                    .HasForeignKey(c => c.WebPageId);
            });

            modelBuilder.Entity<ContentType>(c =>
            {
                c.HasKey(c => c.ContentTypeId);
                c.HasOne(c => c.Contents)
                    .WithMany(w => w.ContentTypes)
                    .HasForeignKey(c => c.ContentId);
            });
        }
    }
}