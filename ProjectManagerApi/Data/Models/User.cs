using ProjectManagerApi.Data.Repositories;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagerApi.Data.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string? LastName { get; set; }

        [Required]
        public byte[]? PasswordHash { get; set; }

        [Required]
        public byte[]? PasswordSalt { get; set; }

        public int Points { get; set; }

        public List<Tech> Technologies { get; set; } = new List<Tech>();
        public List<Language> Laguages { get; set; } = new List<Language>();
    }
}
