using CMS.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Entities
{
  
    public class Content
    {
  
        public int ContentId { get; set; }
    
        public string ContentName { get; set; } // Ger användaren möjlighet att döpa sin content t.ex. "Bakgrundsbild till 'Om Oss sidan'"
 
        public int WebPageId { get; set; }
        public WebPage WebPages { get; set; } 
    }
}
