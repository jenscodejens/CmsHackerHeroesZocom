using CMS.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class ContentType
{
    [Key]
    public int ContentTypeId { get; set; }
    public string ContentString { get; set; }
    public Content Contents { get; set; } = new Content();
    [ForeignKey("ContentId")]
    public int ContentId { get; set; }
    [ForeignKey("UserId")] // ny
    public string UserId { get; set; } = string.Empty;
}
