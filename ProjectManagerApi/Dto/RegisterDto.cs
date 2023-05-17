using ProjectManagerApi.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagerApi.Dto
{
    public class RegisterDto
    {
        [Required]
        [MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = null!;

        [Required]
        [MinLength(4)]
        [MaxLength(50)]
        [Password]
        public string Password { get; set; } = null!;

        [Required]
        public List<int> LanguagesList { get; set; } = null!;

        [Required]
        public List<int> TechnologiesList { get; set; } = null!;
    }
}
