using System.ComponentModel.DataAnnotations;

namespace EncurtadorUrl.Dtos
{
    public class UrlReadDto
    {     
        [Required]
        public string ShortUrl { get; set; }   
    }
}
