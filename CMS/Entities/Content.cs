using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Entities
{
    public class Content
    {
        [Key]
        public int ContentId { get; set; }
        public ContentType ContentType { get; set; }
        public string ContentInput { get; set; }


        //Foregin Keys
        [ForeignKey("WebPage")]
        public int WebPageId { get; set; }
        //public string UserId {  get; set; }
        //public int WebsiteId { get; set; }

        public WebPage? WebPage { get; set; }

    }
}
