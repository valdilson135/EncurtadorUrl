using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EncurtadorUrl.Models
{
    public class UrlModel
    {
        public int Id { get; set; }
        public int Hits { get; set; }

        [Required]
        public string Url { get; set; }
        public string ShortUrl { get; set; }
    }
}
