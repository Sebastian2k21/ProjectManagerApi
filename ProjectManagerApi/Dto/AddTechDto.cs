using System.ComponentModel.DataAnnotations;

namespace ProjectManagerApi.Dto
{
    public class AddTechDto
    {
        [Required]
        public string? Name { get; set; }
    }
}
