using CMS.Data;
using CMS.Entities;
using System.Text.Json; // or Newtonsoft.Json
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CMS.Services
{
    public class ContentService
    {
        private readonly ApplicationDbContext _context;

        public ContentService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Save the text inputs as JSON using a dictionary
        public async Task SaveContentAsync(int contentId, Dictionary<string, string> textInputs, string backgroundcolor, string textcolor)
        {
            var content = await _context.Contents.FirstOrDefaultAsync(c => c.ContentId == contentId);

            if (content != null)
            {
                // Serialize the dictionary to JSON
                content.TextInputsJson = JsonSerializer.Serialize(textInputs); // or JsonConvert.SerializeObject(textInputs) for Newtonsoft.Json
                content.Backgroundcolor = backgroundcolor;
                content.Textcolor = textcolor;

                _context.Update(content);
                await _context.SaveChangesAsync();
            }
        }

        // Retrieve content and deserialize the JSON into a dictionary
        public async Task<Content> GetContentWithTemplateAsync(int contentId)
        {
            var content = await _context.Contents.FirstOrDefaultAsync(c => c.ContentId == contentId);

            if (content != null && !string.IsNullOrEmpty(content.TextInputsJson))
            {
                // Deserialize the JSON back into a dictionary
                //content.TextInputs = JsonSerializer.Deserialize<Dictionary<string, string>>(content.TextInputsJson); // or JsonConvert.DeserializeObject<Dictionary<string, string>>(content.TextInputsJson) for Newtonsoft.Json
            }

            return content;
        }
    }
}
