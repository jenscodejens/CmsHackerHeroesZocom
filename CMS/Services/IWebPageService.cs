using CMS.Entities;

namespace CMS.Services
{
    public interface IWebPageService
    {
        Task<WebPage?> GetWebPageAsync(int webpageId);
        Task<IEnumerable<WebPage>> GetWebPagesFromSameWebSiteAsync(int webpageId);
        Task SaveWebPageAsync(WebPage webpage);
        Task UpdateWebPageAsync(WebPage webpage);
    }
}
