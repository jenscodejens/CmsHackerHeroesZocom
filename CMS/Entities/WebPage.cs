using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Entities
{
    public class WebPage
    {
        [Key]
        public int WebPageId { get; set; }
        [ForeignKey("WebSiteId")]
        public int WebSiteId { get; set; }
        public WebSite WebSites { get; set; } = new WebSite();
        public string Header { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Footer { get; set; } = string.Empty;
        public virtual ICollection<Content> Contents { get; set; } = new List<Content>(); // Virtual för lazyloading
        [ForeignKey("UserId")] // ny
        public string UserId { get; set; } = string.Empty; // ny
    }
}
