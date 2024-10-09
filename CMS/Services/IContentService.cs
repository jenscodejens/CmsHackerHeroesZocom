using CMS.Entities;

namespace CMS.Services
{
    public interface IContentService
    {
        Task<Content?> GetContentAsync(int contentId);
        Task SaveContentAsync(Content content);
        Task UpdateContentAsync(Content content);
    }
}
