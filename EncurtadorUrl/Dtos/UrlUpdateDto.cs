using System.ComponentModel.DataAnnotations;

namespace EncurtadorUrl.Dtos
{
    public class UrlUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Url { get; set; }   
    }
}
