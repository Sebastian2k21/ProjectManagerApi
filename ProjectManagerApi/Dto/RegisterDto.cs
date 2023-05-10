using System.ComponentModel.DataAnnotations;

namespace ProjectManagerApi.Dto
{
    public class RegisterDto
    {
        [Required]
        [MaxLength(50)]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string? LastName { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(50)]
        public string? Password { get; set; }
    }
}
