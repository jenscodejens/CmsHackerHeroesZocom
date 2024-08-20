namespace CMS.Entities
{
    public class WebSite
    {
        public int WebSiteId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateOnly CreationDate { get; set; }
        public DateOnly LastUpdated { get; set; }
    }
}
