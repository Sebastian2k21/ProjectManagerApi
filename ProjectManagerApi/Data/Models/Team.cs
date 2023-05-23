using ProjectManagerApi.Data.Repositories;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagerApi.Data.Models
{
    public class Team : IEntity<int>
    { 
        [Key]
        public int TeamId { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        public int GetId()
        {
            return TeamId;
        }

    }
}
