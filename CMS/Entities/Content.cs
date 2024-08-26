using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Entities
{
    public class Content
    {
        [Key]
        public int ContentId { get; set; }
        public WebPage WebPages { get; set; } = new WebPage();
        public string AddedContent { get; set; }
        [ForeignKey("WebPageId")]
        public int WebPageId { get; set; }
        [ForeignKey("UserId")] // ny
        public string UserId { get; set; } = string.Empty; // ny
        public virtual ICollection<ContentType> ContentTypes { get; set; } = new List<ContentType>();

    }
}
