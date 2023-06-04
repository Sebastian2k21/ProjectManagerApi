using System.ComponentModel.DataAnnotations;

namespace ProjectManagerApi.Dto
{
    public class AddLanguageDto
    {
        [Required]
        public string? Name { get; set; }
    }
}
