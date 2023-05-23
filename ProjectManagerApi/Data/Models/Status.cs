using ProjectManagerApi.Data.Repositories;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagerApi.Data.Models
{
    public class Status : IEntity<int>
    {
        [Key]
        public int StatusId { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }

        public int GetId()
        {
            return StatusId;
        }
    }
}
