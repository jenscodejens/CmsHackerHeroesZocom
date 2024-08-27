using CMS.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CMS.Entities
{

    public abstract class ContentType
    {
        [Key]
        public int ContentTypeId { get; set; }
        [ForeignKey("ContentId")]
        public int ContentId { get; set; }
        public virtual Content Content { get; set; }
        public string DataType { get; set; } // string, array, image om man skulle behöva söka på alla images etc.
    }
}