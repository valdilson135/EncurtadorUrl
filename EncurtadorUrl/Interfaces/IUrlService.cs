using EncurtadorUrl.Dtos;
using EncurtadorUrl.Models;

namespace EncurtadorUrl.Interfaces
{ 
    public interface IUrlService
    {
        Task<IEnumerable<UrlDto>> GetAllUrls();
        Task<UrlDto> GetUrlById(int id);
        Task<UrlDto> GetUrlByUrl(UrlDto url);
        Task<UrlDto> GetUrlByShortUrl(UrlDto url);
        Task<bool> CreateUrl(UrlCreateDto url);
        Task<bool> UpdateUrl(UrlUpdateDto url);
        Task<UrlDto> DeleteUrl(int id); 
        Task<bool> ProcessFile(FileUploadDto file);
        Task<List<UrlModel>> ProcessFile(string file);
        Task<UrlDto> ValidateUrl(UrlReadDto url);
    }
}
