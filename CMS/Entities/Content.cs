using CMS.Models;

namespace CMS.Entities
{

    public class Content
    {
        public int ContentId { get; set; }
        public string ContentName { get; set; } // Ger användaren möjlighet att döpa sin content t.ex. "Bakgrundsbild till 'Om Oss sidan'"
        public int WebPageId { get; set; }

        // New column to store JSON data
        public string TextInputsJson { get; set; }
        //public string? Backgroundcolor { get; set; }
        //public string? Textcolor { get; set; }

        public WebPage WebPages { get; set; }
        public int TemplateId { get; set; }
        public Template Template { get; set; }
    }
}
