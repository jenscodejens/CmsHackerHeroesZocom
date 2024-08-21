using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Entities
{
    public class WebPage
    {
        [Key]
        public int WebPageId { get; set; }
       
        public string Header { get; set; }
        public string Body { get; set; }
        public string Footer { get; set; }

        //Foregin Keys

        //public string UserId { get; set; }

        [ForeignKey("WebSite")]
        public int WebSiteId { get; set; }

        //relationer
        public WebSite? WebSite { get; set; }
        //public ICollection<Content> Content { get; set; } = null;
        public virtual ICollection<Content> Contents { get; set; } = new List<Content>();
    }
}
