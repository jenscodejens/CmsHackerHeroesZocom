using CMS.Entities;
using Microsoft.AspNetCore.Identity;

namespace CMS.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<WebSite> WebSites { get; set; } = new List<WebSite>();
    }

}
