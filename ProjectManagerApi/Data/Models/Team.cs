using System.ComponentModel.DataAnnotations;

namespace ProjectManagerApi.Data.Models
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

    }
}
