using CMS.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Entities
{
    public class WebSite
    {
        [Key]
        public int WebSiteId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateOnly CreationDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly LastUpdated { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = new ApplicationUser();
        public virtual ICollection<WebPage> WebPages { get; set; } = new List<WebPage>(); // Virtual för lazyloading
        [ForeignKey("UserId")]
        public string UserId { get; set; } = string.Empty;
    }
}
