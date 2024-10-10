namespace CMS.Entities;

public class WebSiteVisit
{
    public int Id { get; set; }
    public int WebSiteId { get; set; } 
    public string? PageUrl { get; set; }  
    public int VisitCount { get; set; }
}