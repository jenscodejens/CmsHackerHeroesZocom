using CMS.Models;

namespace CMS.Entities
{
    public class Content
    {
        public int ContentId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int WebPageId { get; set; }
        public int TemplateId { get; set; }
        public string ContentName { get; set; } // Ger användaren möjlighet att döpa sin content t.ex. "Bakgrundsbild till 'Om Oss sidan'"
        public string ContentJson { get; set; }
        public DateOnly CreationDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly? LastUpdated { get; set; }
        public WebPage WebPages { get; set; }
        public Template Template { get; set; }
    }
}
