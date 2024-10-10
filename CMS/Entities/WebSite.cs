using CMS.Data;
using System.ComponentModel.DataAnnotations;

namespace CMS.Entities
{
    public class WebSite
    {
        public int WebSiteId { get; set; }
        public string UserId { get; set; } = string.Empty;
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Url]
        public string? ImageUrl { get; set; } 
        public DateOnly CreationDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly? LastUpdated { get; set; }
        public int? LandingPage { get; set; }
        public int? Header { get; set; }
        public int? Footer { get; set; }
        public ApplicationUser? ApplicationUser { get; set; } 
        public ICollection<WebPage>? WebPages { get; set; }  // Virtual för lazyloading
    }
}
