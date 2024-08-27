using CMS.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Entities
{
    public class Content
    {
        [Key]
        public int ContentId { get; set; }
        public WebPage WebPages { get; set; } = new WebPage();
        public string ContentName { get; set; } = string.Empty; // Ger användaren möjlighet att döpa sin content t.ex. "Bakgrundsbild till 'Om Oss sidan'"
        public virtual ICollection<ContentType> ContentTypes { get; set; } = new List<ContentType>();
        [ForeignKey("WebPageId")]
        public int WebPageId { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; } = string.Empty;
    }
}
