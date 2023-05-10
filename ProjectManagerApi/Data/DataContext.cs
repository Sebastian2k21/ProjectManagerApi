using Microsoft.EntityFrameworkCore;
using ProjectManagerApi.Data.Models;

namespace ProjectManagerApi.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Language> Languages { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectStatus> ProjectStatuses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Tech> Technologies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TeamUser> TeamUsers { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProjectStatus>().HasKey(u => new
            {
                u.StatusId,
                u.ProjectId
            });

            modelBuilder.Entity<TeamUser>().HasKey(u => new
            {
                u.UserId,
                u.TeamId,
                u.RoleId
            });

            new Seeder(modelBuilder)
                .SeedRoles()
                .SeedLanguages()
                .SeedTechnologies()
                .SeedStatuses();
        }
    }
}
