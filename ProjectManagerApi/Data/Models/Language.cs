using ProjectManagerApi.Data.Repositories;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagerApi.Data.Models
{
    public class Language : IEntity<int>
    {
        [Key]
        public int LanguageId { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }

        public List<Project> Projects { get; set; } = new List<Project>();
        public List<User> Users { get; set; } = new List<User>();

        public int GetId()
        {
            return LanguageId;
        }
    }
}
