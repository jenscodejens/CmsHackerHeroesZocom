using CMS.Data;
using CMS.Entities;
using System.Text.Json; // or Newtonsoft.Json
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CMS.Services
{
    public class ContentService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        // Inject the IDbContextFactory instead of ApplicationDbContext directly
        public ContentService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task SaveContentAsync(string contentName, int webPageId, Dictionary<string, string> textInputs, string backgroundColor, string textColor, int templateId)
        {
            // Create a new context instance on-demand using the factory
            await using var context = _dbContextFactory.CreateDbContext();

            var content = new Content
            {
                ContentName = contentName,
                WebPageId = webPageId,
                TextInputsJson = JsonSerializer.Serialize(textInputs), // or use Newtonsoft.Json
                Backgroundcolor = backgroundColor,
                Textcolor = textColor,
                TemplateId = templateId
            };

            context.Contents.Add(content);
            await context.SaveChangesAsync();
        }
    }
}
