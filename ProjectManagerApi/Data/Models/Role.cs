using ProjectManagerApi.Data.Repositories;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagerApi.Data.Models
{
    public class Role : IEntity<int>
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }

        public int GetId()
        {
            return RoleId;
        }
    }
}
