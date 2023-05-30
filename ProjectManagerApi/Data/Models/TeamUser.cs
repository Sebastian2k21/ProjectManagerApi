using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagerApi.Data.Models
{
    public class TeamUser
    {
        public User? User { get; set; }
        public Team? Team { get; set; }
        public Role? Role { get; set; }

        [ForeignKey(nameof(User))]
        public int? UserId { get; set; }

        [ForeignKey(nameof(Team))]
        public int? TeamId { get; set; }

        [ForeignKey(nameof(Role))]
        public int? RoleId { get; set; }
    }
}
