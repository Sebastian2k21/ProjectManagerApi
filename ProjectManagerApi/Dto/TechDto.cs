using System.ComponentModel.DataAnnotations;

namespace ProjectManagerApi.Dto
{
    public class TechDto
    {
        [Required]
        public int TechId { get; set;}
        [Required]
        public string? Name { get; set;}
    }
}
