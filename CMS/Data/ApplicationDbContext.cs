using CMS.Entities;
using CMS.Models;
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
        public DbSet<WebSite> WebSites { get; set; }
        public DbSet<WebSiteVisit> WebSiteVisits { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<ContentRenderingOrder> ContentRenderingOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // WebSite configuration
            modelBuilder.Entity<WebSite>(e =>
            {
                e.ToTable("WebSites");
                e.HasKey(e => e.WebSiteId);
                e.HasOne(e => e.ApplicationUser)
                  .WithMany(u => u.WebSites)
                  .HasForeignKey(e => e.UserId);
            });

            // WebPage configuration
            modelBuilder.Entity<WebPage>(p =>
            {
                p.ToTable("WebPages");
                p.HasKey(p => p.WebPageId);
                p.HasOne(p => p.WebSite)
                  .WithMany(w => w.WebPages)
                  .HasForeignKey(p => p.WebSiteId)
                  .OnDelete(DeleteBehavior.NoAction);

                p.HasOne(p => p.ApplicationUser)
                  .WithMany()
                  .HasForeignKey(p => p.UserId);

                p.HasMany(p => p.ContentRenderingOrders)
                  .WithOne(cro => cro.WebPage)
                  .HasForeignKey(cro => cro.WebPageId);
            });

            // Content configuration
            modelBuilder.Entity<Content>(c =>
            {
                c.ToTable("Contents");
                c.HasKey(c => c.ContentId);
                c.HasOne(c => c.WebPages)
                  .WithMany()
                  .HasForeignKey(c => c.WebPageId);
                c.HasOne(c => c.Template)
                  .WithMany() // No reverse navigation
                  .HasForeignKey(c => c.TemplateId);
                c.HasOne(c => c.ApplicationUser)
                  .WithMany()
                  .HasForeignKey(c => c.UserId) //eeeee
                  .OnDelete(DeleteBehavior.NoAction);
            });

            // ContentRenderingOrder configuration
            modelBuilder.Entity<ContentRenderingOrder>(c =>
            {
                c.ToTable("ContentRenderingOrders");
                c.HasKey(cro => cro.ContentOrderId);

                // Set ContentId to null if its content is deleted
                c.HasOne(cro => cro.Content)
                   .WithMany()
                   .HasForeignKey(cro => cro.ContentId)
               // Set it to null for now, the update of this table has to be done with logic in delete content functionality
                   .OnDelete(DeleteBehavior.SetNull);

                c.HasOne(cro => cro.WebPage)
                   .WithMany(wp => wp.ContentRenderingOrders)
                   .HasForeignKey(cro => cro.WebPageId)
                   // https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.deletebehavior?view=efcore-8.0
                   // Need to sort later, unsure of which field to use with on .OnDelete but Restrict at least doesnt fuck things up
                   .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
