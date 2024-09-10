using CMS.Data;

namespace CMS.Entities
{

    public class WebSite
    {
        public int WebSiteId { get; set; }
        public string Title { get; set; } 
        public string Description { get; set; }
        public DateOnly CreationDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly? LastUpdated { get; set; }
       
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser ApplicationUser { get; set; } 
        public ICollection<WebPage> WebPages { get; set; }  // Virtual för lazyloading
   
    }
}
