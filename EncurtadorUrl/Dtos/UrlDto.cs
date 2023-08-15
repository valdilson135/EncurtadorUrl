using System.ComponentModel.DataAnnotations;

namespace EncurtadorUrl.Dtos
{
    public class UrlDto
    {
        public int Id { get; set; }
        public int Hits { get; set; }

        [Required]
        public string Url { get; set; }   
        public string ShortUrl { get; set; }
    }
}
