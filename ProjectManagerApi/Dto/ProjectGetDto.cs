using ProjectManagerApi.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagerApi.Dto
{
    public class ProjectGetDto
    {
        public int ProjectId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Requirements { get; set; }

        public int DifficultyLevel { get; set; }

        public DateTime? FinishDate { get; set; }

        public DateTime? SubmissionDate { get; set; }

        public int? TeamId { get; set; }

        public bool PrivateRecruitment { get; set; }

        public List<int> Languages { get; set; } = new List<int>();
        public List<int> Technologies { get; set; } = new List<int>();

    }
}