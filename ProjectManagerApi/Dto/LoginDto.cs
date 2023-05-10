using System.ComponentModel.DataAnnotations;

namespace ProjectManagerApi.Dto
{
    public class LoginDto
    {
        [Required]
        [MaxLength(50)]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(50)]
        public string? Password { get; set; }
    }
}
