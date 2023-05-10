using Microsoft.EntityFrameworkCore;
using ProjectManagerApi.Data.Models;

namespace ProjectManagerApi.Data
{
    public class Seeder
    {
        private ModelBuilder modelBuilder;

        public Seeder(ModelBuilder modelBuilder)
        {
            this.modelBuilder = modelBuilder;
        }

        public Seeder SeedRoles()
        {
            modelBuilder.Entity<Role>().HasData(
                               new Role { RoleId = 1, Name = "Developer" },
                                              new Role { RoleId = 2, Name = "Tester" },
                                              new Role { RoleId = 3, Name = "Leader" },
                                              new Role { RoleId = 4, Name = "DevOps" }
                                                         );
            return this;
        }


        public Seeder SeedStatuses()
        {
            modelBuilder.Entity<Status>().HasData(
                                              new Status { StatusId = 1, Name = "Created" },
                                    new Status { StatusId = 2, Name = "Team Completed" },
                                    new Status { StatusId = 3, Name = "Development" },
                                    new Status { StatusId = 4, Name = "Tested" },
                                    new Status { StatusId = 5, Name = "Done" }
                                    );
            return this;
        }

        public Seeder SeedLanguages()
        {
            modelBuilder.Entity<Language>().HasData(
                new Language { LanguageId = 1, Name = "C#" },
                new Language { LanguageId = 2, Name = "Java" },
                new Language { LanguageId = 3, Name = "Python" },
                new Language { LanguageId = 4, Name = "C++" },
                new Language { LanguageId = 5, Name = "Javascript" }
                );
            return this;
        }

        public Seeder SeedTechnologies()
        {
            modelBuilder.Entity<Tech>().HasData(
                new Tech { TechId=1, Name="ASP.NET"},
                new Tech { TechId=2, Name="UWP"},
                new Tech { TechId=3, Name="Selenium"},
                new Tech { TechId=4, Name="Entity Framework"},
                new Tech { TechId=5, Name="React"}
                );
            return this;
        }
    }
}
