using ProjectManagerApi.Data.Repositories;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagerApi.Data.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }
    }
}
