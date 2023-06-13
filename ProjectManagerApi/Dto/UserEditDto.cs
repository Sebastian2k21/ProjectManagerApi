using ProjectManagerApi.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagerApi.Dto
{
    public class UserEditDto
    {
        public int Id { get; set; }

        public string? Email { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        
        public List<LanguageDto> Languages { get; set; }

        public List<TechDto> Technologies { get; set; }
    }
}
