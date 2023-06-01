using System.ComponentModel.DataAnnotations;

namespace ProjectManagerApi.Dto
{
    public class ProjectStatusDto
    {
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public int StatusId { get; set; }
    }
}
