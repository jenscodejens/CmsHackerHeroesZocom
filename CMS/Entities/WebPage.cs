using CMS.Data;
using System.ComponentModel.DataAnnotations;

namespace CMS.Entities
{
    public class WebPage
    {
        public int WebPageId { get; set; }
        public int WebSiteId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Title { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateOnly CreationDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly? LastUpdated { get; set; }
        public string VisitorUrl { get; set; } = string.Empty;
        public WebSite WebSite { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        //public ICollection<Content> Contents { get; set; }

        // TODO: använd inte new nedan, kolla upp videon från youtube igen om bättre lösning
        public ICollection<ContentRenderingOrder> ContentRenderingOrders { get; set; } = new List<ContentRenderingOrder>();
    }
}
