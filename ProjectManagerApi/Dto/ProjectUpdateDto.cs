using System.ComponentModel.DataAnnotations;

namespace ProjectManagerApi.Dto
{
    public class ProjectUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(1000)]
        public string? Requirements { get; set; }

        [Required]
        public int DifficultyLevel { get; set; }

        public DateTime? FinishDate { get; set; }

        [Required]
        public bool PrivateRecruitment { get; set; }
    }
}
