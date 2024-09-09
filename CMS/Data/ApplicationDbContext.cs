using CMS.Entities;
using CMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Content> Contents { get; set; }
        public DbSet<WebPage> WebPages { get; set; }
        public DbSet<WebSite> WebSites { get; set; }
        public DbSet<Template> Templates { get; set; }  // Map to the Templates table


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WebSite>(e =>
            {
                e.ToTable("WebSites");
                e.HasKey(e => e.WebSiteId);
                e.HasOne(e => e.ApplicationUser)
                  .WithMany(u => u.WebSites)
                  .HasForeignKey(e => e.UserId);
            });

            modelBuilder.Entity<WebPage>(p =>
            {
                p.ToTable("WebPages");
                p.HasKey(p => p.WebPageId);
                p.HasOne(p => p.WebSite)
                  .WithMany(w => w.WebPages)
                  .HasForeignKey(p => p.WebSiteId);

            });

            modelBuilder.Entity<Content>(c =>
            {
                c.ToTable("Contents");
                c.HasKey(c => c.ContentId);
                c.HasOne(c => c.WebPages)
                    .WithMany(w => w.Contents)
                    .HasForeignKey(c => c.WebPageId);
                c.HasOne(c => c.Template)
                 .WithMany()  // No reverse navigation
                 .HasForeignKey(c => c.TemplateId);
            });
        }
    }
}
