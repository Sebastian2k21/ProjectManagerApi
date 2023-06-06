using ProjectManagerApi.Data.Repositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagerApi.Data.Models
{
    public class Project 
    {
        [Key]
        public int ProjectId { get; set; }

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

        public DateTime? SubmissionDate { get; set; }

        [Required]
        public Team? Team { get; set; }

        [ForeignKey(nameof(Team))]
        public int? TeamId { get; set; }

        [Required]
        public bool PrivateRecruitment { get; set; }

        public List<User> Applicants { get; set; } = new List<User>();

        public List<Language> Languages { get; set; } = new List<Language>();
        public List<Tech> Technologies { get; set; } = new List<Tech>();
    }
}
