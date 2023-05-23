using ProjectManagerApi.Data.Repositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagerApi.Data.Models
{
    public class ProjectStatus
    {
        [Required]
        public Project? Project { get; set; }

        [Required]
        public Status? Status { get; set; }

        [ForeignKey(nameof(Project))]
        public int? ProjectId { get; set; }

        [ForeignKey(nameof(Status))]
        public int? StatusId { get; set; }

        public DateTime? DateCreated { get; set; } = DateTime.Now;
    }
}
