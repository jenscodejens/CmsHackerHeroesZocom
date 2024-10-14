using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Entities
{
    public class ContentRenderingOrder
    {
        [NotMapped]
        public int ContentOrderId { get; set; }
        public int Order { get; set; }
        public int? ContentId { get; set; }
        public int WebPageId { get; set; }

        // Navigation properties
        public Content Content { get; set; }
        public WebPage WebPage { get; set; }
    }
}
