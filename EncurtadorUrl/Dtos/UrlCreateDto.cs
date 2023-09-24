using System.ComponentModel.DataAnnotations;

namespace EncurtadorUrl.Dtos
{
    public class UrlCreateDto
    {     
        [Required]
        public string Url { get; private set; }   

    }
}
