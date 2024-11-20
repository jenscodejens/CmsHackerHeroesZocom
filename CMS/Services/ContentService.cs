﻿using CMS.Data;
using CMS.Entities;
using Microsoft.EntityFrameworkCore;

namespace CMS.Services
{
    public class ContentService : IContentService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
        private readonly IGetCurrentUserService _currentUserService;

        public ContentService(IDbContextFactory<ApplicationDbContext> dbContextFactory, IGetCurrentUserService currentUserService)
        {
            _dbContextFactory = dbContextFactory;
            _currentUserService = currentUserService;
        }

        // Method to retrieve content by ID
        public async Task<Content?> GetContentAsync(int contentId)
        {
            await using var context = _dbContextFactory.CreateDbContext();
            return await context.Contents.FirstOrDefaultAsync(c => c.ContentId == contentId);
        }

        // Method to save new content to the database
        public async Task SaveContentAsync(Content content)
        {
            await using var context = _dbContextFactory.CreateDbContext();

            // Ensure WebPageId exists in the WebPages table
            var webPageExists = await context.WebPages.AnyAsync(wp => wp.WebPageId == content.WebPageId);
            if (!webPageExists)
            {
                throw new InvalidOperationException($"WebPageId {content.WebPageId} does not exist.");
            }

            context.Contents.Add(content);

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
        public async Task UpdateContentAsync(Content content)
        {
            await using var context = _dbContextFactory.CreateDbContext();

            var existingContent = await context.Contents.FirstOrDefaultAsync(c => c.ContentId == content.ContentId);
            if (existingContent == null)
            {
                throw new InvalidOperationException($"Content with ID {content.ContentId} does not exist.");
            }

            existingContent.ContentName = content.ContentName;
            existingContent.ContentJson = content.ContentJson;
            existingContent.LastUpdated = DateOnly.FromDateTime(DateTime.Now);

            context.Contents.Update(existingContent);

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

        /// <summary>
        /// Gets the next usable integer that can be saved to RenderingOrderPosition in the Content table in the Db
        /// </summary>
        /// <param name="webPageId">The unique webpage ID.</param>
        /// <returns>The RenderingOrderPosition value to be used</returns>
        public async Task<int> GetNextRenderingOrderAsync(int webPageId)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();

            // Get the maximum current RenderingOrderPosition for the specific WebPageId
            int maxOrder = await dbContext.Contents
                .Where(c => c.WebPageId == webPageId)
                .MaxAsync(c => (int?)c.RenderingOrderPosition) ?? 0;

            // Return the next order number
            return maxOrder + 1;
        }
    }

}
