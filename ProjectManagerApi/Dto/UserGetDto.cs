using System.ComponentModel.DataAnnotations;

namespace ProjectManagerApi.Dto
{
    public class UserGetDto
    {
        public int Id { get; set; }

        public string? Email { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public int Points { get; set; }
    }
}
