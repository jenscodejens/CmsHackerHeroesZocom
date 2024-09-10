namespace CMS.Entities
{
  
    public class WebPage
    {
        public int WebPageId { get; set; }
        public int WebSiteId { get; set; }
        public WebSite WebSite { get; set; }
        public string Header { get; set; } 
        public string Body { get; set; } 
        public string Footer { get; set; } 
        public ICollection<Content> Contents { get; set; }
    }
}
