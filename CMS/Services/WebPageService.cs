using CMS.Data;
using CMS.Entities;
using Microsoft.EntityFrameworkCore;

namespace CMS.Services
{
    public class WebPageService : IWebPageService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
        private readonly IGetCurrentUserService _currentUserService;

        public WebPageService(IDbContextFactory<ApplicationDbContext> dbContextFactory, IGetCurrentUserService currentUserService)
        {
            _dbContextFactory = dbContextFactory;
            _currentUserService = currentUserService;
        }

        // Method to retrieve content by ID
        public async Task<WebPage?> GetWebPageAsync(int webPageId)
        {
            await using var context = _dbContextFactory.CreateDbContext();
            return await context.WebPages.FirstOrDefaultAsync(c => c.WebPageId == webPageId);
        }

        public async Task<IEnumerable<WebPage>> GetWebPagesFromSameWebSiteAsync(int webPageId)
        {
            await using var context = _dbContextFactory.CreateDbContext();

            var webPageExists = await context.WebPages.AnyAsync(wp => wp.WebPageId == webPageId);

            if (!webPageExists)
            {
                throw new InvalidOperationException($"WebPageId {webPageId} does not exist.");
            }

            var pages = await context.WebPages.ToListAsync();
            var page = pages.FirstOrDefault(wp => wp.WebPageId == webPageId);
            var webpages = pages.Where(wp => wp.WebSiteId == page.WebSiteId);

            if (webpages==null)
            {
                throw new InvalidOperationException($"WebPageId {webPageId} does not exist.");
            }

            return webpages;


        }
    

        // Method to save new content to the database
        public async Task SaveWebPageAsync(WebPage webpage)
        {
            await using var context = _dbContextFactory.CreateDbContext();

            // Ensure WebPageId exists in the WebPages table
            var webPageExists = await context.WebSites.AnyAsync(ws => ws.WebSiteId == webpage.WebSiteId);
            if (!webPageExists)
            {
                throw new InvalidOperationException($"WebPageId {webpage.WebPageId} does not exist.");
            }

            context.WebPages.Add(webpage);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error: {ex.InnerException?.Message}");
                throw;
            }
        }

        // Method to update existing content in the database
        public async Task UpdateWebPageAsync(WebPage webpage)
        {
            await using var context = _dbContextFactory.CreateDbContext();

            var existingWebPage = await context.WebPages.FirstOrDefaultAsync(wp => wp.WebPageId == webpage.WebPageId);
            if (existingWebPage == null)
            {
                throw new InvalidOperationException($"Web page with ID {webpage.WebPageId} does not exist.");
            }

            existingWebPage.Title = webpage.Title;
            existingWebPage.VisitorUrl = webpage.VisitorUrl;
            existingWebPage.Description = webpage.Description;
            existingWebPage.LastUpdated = DateOnly.FromDateTime(DateTime.Now);

            context.WebPages.Update(existingWebPage);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error: {ex.InnerException?.Message}");
                throw;
            }
        }
    }

}
