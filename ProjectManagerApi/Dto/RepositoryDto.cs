using System.ComponentModel.DataAnnotations;

namespace ProjectManagerApi.Dto
{
    public class RepositoryDto
    {
        [Required]
        public int ProjectId { get; set; }

        [Required]
        [MaxLength(200)]
        public string RepositoryUrl { get; set; } = null!;
    }
}
