namespace CMS.Entities
{
    public class Content
    {
        public int Id { get; set; }
        public ContentType ContentType { get; set; }
        public string ContentInput { get; set; }
        public string UserId {  get; set; }
        public int WebsiteId { get; set; }
    }
}
