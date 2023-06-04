using System.ComponentModel.DataAnnotations;

namespace ProjectManagerApi.Dto
{
    public class LanguageDto
    {
        [Required]
        public int LanguageId { get; set; }

        [Required]
        public string? Name { get; set; }
    }
}
