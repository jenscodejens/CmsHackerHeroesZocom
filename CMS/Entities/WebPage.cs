using System.ComponentModel.DataAnnotations;

namespace CMS.Entities
{
  
    public class WebPage
    {
        public int WebPageId { get; set; }
        public int WebSiteId { get; set; }
        public WebSite WebSite { get; set; }
        [Required]
        public string Header { get; set; } 
        
        [Url]
        public string? ImageUrl { get; set; } 

        [Required]
        public string Body { get; set; } 
        [Required]
        public string Footer { get; set; } 
        public ICollection<Content> Contents { get; set; }
        public string? WebPageName { get; set; }
    }
}
