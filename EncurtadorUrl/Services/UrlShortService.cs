using EncurtadorUrl.Interfaces;
using NanoidDotNet;

namespace EncurtadorUrl.Services
{
    public class UrlShortService : IUrlShortService
    {
        private const int IdLenghtShort = 5;
        public string SetUrlShort()
        {
            string shortId = Nanoid.Generate("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890", IdLenghtShort);
            string shortUrl = $"http://chr.dc/{shortId}";

            return shortUrl;
        }
    }
}
 21