using EncurtadorUrl.Models;

namespace EncurtadorUrl.Interfaces
{
    public interface IUrlRepository
    {
        Task<int> SaveChanges();
        Task<IEnumerable<UrlModel>> GetAllUrls();
        Task<UrlModel> GetUrlById(int id);
        Task<UrlModel> GetUrlByUrl(UrlModel url);
        Task<UrlModel> GetUrlByShortUrl(UrlModel url);
        Task CreateUrl(UrlModel url);
        Task UpdateUrl(UrlModel url);
        Task DeleteUrl(UrlModel url);
    }
}
