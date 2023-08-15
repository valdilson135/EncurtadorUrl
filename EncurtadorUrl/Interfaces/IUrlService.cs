using EncurtadorUrl.Dtos;
using EncurtadorUrl.Models;

namespace EncurtadorUrl.Interfaces
{ 
    public interface IUrlService
    {
        Task<IEnumerable<UrlModel>> GetAllUrls();
        Task<UrlModel> GetUrlById(int id);
        Task<UrlModel> GetUrlByUrl(UrlModel url);
        Task<UrlModel> GetUrlByShortUrl(UrlModel url);
        Task<bool> CreateUrl(UrlModel url);
        Task<bool> UpdateUrl(UrlModel url);
        Task DeleteUrl(UrlModel url);
        Task<bool> ProcessFile(FileUploadDto file);
    }
}
