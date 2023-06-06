using System.ComponentModel.DataAnnotations;

namespace ProjectManagerApi.Dto
{
    public class ApplyToProjectDto
    {
        [Required]
        public int ProjectId { get; set; }
    }
}
