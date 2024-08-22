using CMS.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace CMS.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<WebSite> WebSite { get; set; }
        public DbSet<WebPage> WebPage { get; set; }
        public DbSet<Content> Content { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //User -> Website(One-to-Many)
            builder.Entity<WebSite>(e =>
            {
                e.HasKey(e => e.WebSiteId);
                e.HasOne(e => e.ApplicationUser)
                 .WithMany(e => e.WebSites)
                 .HasForeignKey(e => e.UserId);
            });

            //Website -> webpage(One-to-One) 
            builder.Entity<WebPage>(e => {
                e.HasKey(e=>e.WebPageId);
                e.HasOne(e => e.WebSite)
                    .WithOne(e => e.WebPage);
            });

            //WebPage -> Content(One-to-many)
            builder.Entity<Content>(e =>
            {
                e.HasKey(e => e.ContentId);
                e.HasOne(e => e.WebPage).WithMany(e => e.Contents).HasForeignKey(e => e.WebPageId);
            });


        }
    }
}
