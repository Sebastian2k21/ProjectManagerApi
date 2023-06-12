using Microsoft.AspNetCore.Components.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ProjectManagerApi.Data;
using ProjectManagerApi.Data.Models;
using ProjectManagerApi.Dto;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;

namespace UnitTests
{
    public class IntegrationTests: IClassFixture<ProjectManagerAppTestFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly ProjectManagerAppTestFactory<Program> _app;
        private readonly DataContext _context;

        public IntegrationTests(ProjectManagerAppTestFactory<Program> app )
        {
            _app = app;
            _client = app.CreateClient();
            using (var scope = app.Services.CreateScope())
            {
                _context = scope.ServiceProvider.GetService<DataContext>();

                #region 
                var language = new List<Language>()
                {
                    new()
                    {
                        LanguageId= 1,
                        Name = "C#"
                    },
                    new()
                    {
                        LanguageId= 2,
                        Name = "C++"
                    }
                };

                var tech = new List<Tech>()
                {
                    new()
                    {
                        TechId= 1,
                        Name = "React"
                    },
                    new()
                    {
                        TechId= 2,
                        Name="MongoDB"
                    }
                };

                string password = "A12345";

                byte[] GenerateSalt()
                {
                    byte[] salt = new byte[16];
                    using (var rng = new RNGCryptoServiceProvider())
                    {
                        rng.GetBytes(salt);
                    }
                    return salt;
                }

                byte[] ConcatenateBytes(byte[] byteArray1, byte[] byteArray2)
                {
                    byte[] combinedBytes = new byte[byteArray1.Length + byteArray2.Length];
                    Buffer.BlockCopy(byteArray1, 0, combinedBytes, 0, byteArray1.Length);
                    Buffer.BlockCopy(byteArray2, 0, combinedBytes, byteArray1.Length, byteArray2.Length);
                    return combinedBytes;
                }

                byte[] GenerateHash(byte[] inputBytes)
                {
                    using (var sha512 = SHA512.Create())
                    {
                        return sha512.ComputeHash(inputBytes);
                    }
                }
                byte[] saltBytes = GenerateSalt();

                // Tworzenie hasha
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] saltedPasswordBytes = ConcatenateBytes(passwordBytes, saltBytes);
                byte[] hashedPasswordBytes = GenerateHash(saltedPasswordBytes);

                #endregion

                //Uzytkownicy
                _context.Users.Add(
                    new User
                    {
                        Id = 1,
                        Email = "test@test.pl",
                        FirstName = "Adrian",
                        LastName = "Test",
                        Points = 0,
                        Technologies = tech,
                        Laguages = language,
                        PasswordSalt = saltBytes,
                        PasswordHash = hashedPasswordBytes,
                    });
                _context.Users.Add(
                    new User
                    {
                        Id = 2,
                        Email = "test2@test.pl",
                        FirstName = "Mateusz",
                        LastName = "Test",
                        Points = 0,
                        Technologies = tech,
                        Laguages = language,
                        PasswordSalt = saltBytes,
                        PasswordHash = hashedPasswordBytes,
                    });
                _context.Users.Add(
                    new User
                    {
                        Id = 3,
                        Email = "test3@test.pl",
                        FirstName = "Tomasz",
                        LastName = "Test",
                        Points = 0,
                        Technologies = tech,
                        Laguages = language,
                        PasswordSalt = saltBytes,
                        PasswordHash = hashedPasswordBytes,
                    });
                _context.Users.Add(
                    new User
                    {
                        Id = 4,
                        Email = "test4@test.pl",
                        FirstName = "Pawe³",
                        LastName = "Test",
                        Points = 0,
                        Technologies = tech,
                        Laguages = language,
                        PasswordSalt = saltBytes,
                        PasswordHash = hashedPasswordBytes,
                    });

                //Jezyki
                _context.Languages.Add(
                    new Language
                    {
                        LanguageId = 1,
                        Name = "C#",
                    });

                _context.Languages.Add(
                    new Language
                    {
                        LanguageId = 2,
                        Name = "C++"
                    });

                //Technologie
                _context.Technologies.Add(
                    new Tech
                    {
                        TechId = 1,
                        Name = "React",
                    });
                _context.Technologies.Add(
                    new Tech
                    {
                        TechId = 2,
                        Name = "MongoDb",
                    });

                //Role w projektach
                _context.Roles.Add(
                    new Role
                    {
                        RoleId = 1,
                        Name = "Developer",
                    });
                _context.Roles.Add(
                   new Role
                   {
                       RoleId = 2,
                       Name = "Tester",
                   });
                _context.Roles.Add(
                   new Role
                   {
                       RoleId = 3,
                       Name = "Leader",
                   });
                _context.Roles.Add(
                  new Role
                  {
                      RoleId = 4,
                      Name = "DevOps",
                  });

                //Statusy projektów
                _context.Statuses.Add(
                    new Status
                    {
                        StatusId = 1,
                        Name = "Created",
                    });
                _context.Statuses.Add(
                    new Status
                    {
                        StatusId = 2,
                        Name = "Team Completed",
                    });
                _context.Statuses.Add(
                    new Status
                    {
                        StatusId = 3,
                        Name = "Development",
                    });
                _context.Statuses.Add(
                    new Status
                    {
                        StatusId = 4,
                        Name = "Tested",
                    });
                _context.Statuses.Add(
                    new Status
                    {
                        StatusId = 5,
                        Name = "Done",
                    });

                //Team
                _context.Add(
                    new Team
                    {
                        TeamId = 1,
                        Name = "Test Team",
                    });

                //TeamUsers
                _context.Add(
                    new TeamUser
                    {
                        UserId = 1,
                        TeamId = 1,
                        RoleId = 3,
                    });

                //Projekt
                _context.Add(
                    new Project
                    {
                        ProjectId = 1,
                        Name = "Test",
                        Description = "Test",
                        Requirements = "Test",
                        DifficultyLevel = 2,
                        FinishDate = DateTime.Now,
                        SubmissionDate = DateTime.Now,
                        TeamId = 1,
                        PrivateRecruitment = false,
                    });

                _context.Add(
                    new ProjectStatus
                    {
                        ProjectId = 1,
                        StatusId = 1,
                        DateCreated = DateTime.Now,
                        ProjectStatusId= 1,
                    });

                _context.SaveChanges();




            }
        }


        }
    }
}