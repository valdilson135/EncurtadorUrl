using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EncurtadorUrl.Models
{
    public class UrlModel
    {
        public UrlModel(string url)
        {
            Url = url;
        }
        public int Id { get; set; }
        public int Hits { get; private set; }

        public string Url { get; set; }
        public string ShortUrl { get;  private set; }

        public void SetHints(int hints)
        {  Hits += hints; }

        public void SetShortUrl(string shortUrl)
        {
            ShortUrl = shortUrl.ToLower();
        }
    }
}
