namespace CMS.Entities
{
    public class WebPage
    {
        public int PageId { get; set; }
        public int WebSiteId { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        public string UserId { get; set; }
    }
}
