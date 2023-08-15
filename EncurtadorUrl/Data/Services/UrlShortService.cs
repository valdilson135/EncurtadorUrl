using EncurtadorUrl.Dtos;
using EncurtadorUrl.Interfaces;
using NanoidDotNet;

namespace EncurtadorUrl.Data.Services
{
    public class UrlShortService : IUrlShortService
    {
        private const int IdLenghtShort = 5;       
        public string SetUrlShort(UrlCreateDto url)
        {
            string shortId = Nanoid.Generate("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890", IdLenghtShort);
            string shortUrl = $"http://chr.dc/{shortId}";

            return shortUrl;
        }
    }
}
