using Microsoft.AspNetCore.Components.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ProjectManagerApi.Data;
using ProjectManagerApi.Data.Models;
using ProjectManagerApi.Dto;
using System.Net;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper.Configuration.Annotations;
using System.Net.WebSockets;

namespace UnitTests
{
    public class IntegrationTests : IClassFixture<ProjectManagerAppTestFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly ProjectManagerAppTestFactory<Program> _app;
        private readonly DataContext _context;

        public IntegrationTests(ProjectManagerAppTestFactory<Program> app)
        {
            _app = app;
            _client = app.CreateClient();
            using (var scope = app.Services.CreateScope())
            {
                _context = scope.ServiceProvider.GetService<DataContext>();

                #region ==Metody==
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




                #endregion Db==


                #region ==Db==
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
                    FirstName = "Pawe³",
                    LastName = "Test",
                    Points = 0,
                    Technologies = tech,
                    Laguages = language,
                    PasswordSalt = saltBytes,
                    PasswordHash = hashedPasswordBytes,
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
                _context.Add(
                    new TeamUser()
                    {
                        UserId = 2,
                        TeamId = 1,
                        RoleId = 1,

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
                        ProjectStatusId = 1,
                    });

                _context.SaveChangesAsync();



            }
            #endregion 
        }

        #region JwtGeneration
        string GenerateJwtToken(string userId, string username, string secretKey)
        {
            var claims = new[]
            {
                        new Claim(ClaimTypes.NameIdentifier, userId),
                        new Claim(ClaimTypes.Email, username),
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "projectmanager",
                audience: "projectmanager",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
        void Athrz()
        {
            var token = GenerateJwtToken("1", "test@test.pl", "fdjfdshjkfdsjfdf45ds1f3d2sf1e5fdsnf");

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
        #endregion

        [Fact]
        public async void GetAllProjects_ShouldReturnOneProject()
        {
            Athrz();
            var result = await _client.GetFromJsonAsync<List<ProjectDto>>("api/project/get-all-projects");
            if (result != null)
            {
                Assert.Single(result);
                Assert.Equal("Test", result[0].Name);
            }

        }

        [Fact]
        public async void GetAllProjects_ShouldReturnOkStatus()
        {
            Athrz();
            var result = await _client.GetAsync("api/project/get-all-projects");
            Assert.Equal(HttpStatusCode.OK , result.StatusCode);
            Assert.Contains("application/json", result.Content.Headers.GetValues("Content-Type").First());
        }

        [Fact]
        public async void GetAllProjects_ShouldReturnUnauthorized()
        {
            var result = await _client.GetAsync("api/project/get-all-projects");
            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
        }

        [Fact]
        public async void GetProjectById_ShouldReturnProject()
        {
            var projectId = 1;
            Athrz();

            var result = await _client.GetFromJsonAsync<Project>($"api/project/{projectId}");

            if(result != null)
            {
                Assert.Equal(projectId, result.ProjectId);
            }

        }
        [Fact]
        public async void GetProjectById_ShouldReturnOk()
        {
            var projectId = 1;
            Athrz();

            var result = await _client.GetAsync($"api/project/{projectId}");

            Assert.Equal(HttpStatusCode.OK,result.StatusCode);
            Assert.Contains("application/json", result.Content.Headers.GetValues("Content-Type").First());
        }
        [Fact]
        public async void GetProjectById_ShouldReturnUnauthorized()
        {
            var projectId = 1;

            var result = await _client.GetAsync($"api/project/{projectId}");
            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);

        }
        [Fact]
        public async void AddUserToProject_ShouldReturnOk()
        {
            Athrz();

            var addUserDto = new TeamUserDto()
            {
                ProjectId = 1,
                UserId= 2,
                RoleId= 1,
            };

            var content = JsonContent.Create(addUserDto);

            var response = await _client.PostAsync("api/project/add-user",content);
        }
        [Fact]
        public async Task AddUserToProject_ShouldReturnBadRequest()
        {
            Athrz();
            var addUserDto = new TeamUserDto
            {
                
                ProjectId = 1,
                UserId = 0, 
                RoleId = 3 
            };

            var content = JsonContent.Create(addUserDto);

            var response = await _client.PostAsync("api/project/add-user", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task AddUserToProject_ShouldReturnUnauthorized()
        {
            var addUserDto = new TeamUserDto
            {

                ProjectId = 1,
                UserId = 2,
                RoleId = 3
            };

            var content = JsonContent.Create(addUserDto);

            var response = await _client.PostAsync("api/project/add-user", content);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
     
     
        
       
        [Fact]
        public async Task ChangeRole_ShouldReturnOk()
        {
            Athrz();
            var addUserDto = new TeamUserDto
            {
                ProjectId = 1,
                UserId = 2,
                RoleId = 4,
            };

            var content = JsonContent.Create(addUserDto);

            var response = await _client.PutAsync("api/project/change-role", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }
        [Fact]
        public async Task ChangeRole_ShouldReturnBadRequest_NotExisitingProject()
        {
            Athrz();
            var addUserDto = new TeamUserDto
            {
                ProjectId = 2,
                UserId = 2,
                RoleId = 3,
            };

            var content = JsonContent.Create(addUserDto);

            var response = await _client.PutAsync("api/project/change-role", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        }

        [Fact]
        public async Task ChangeRole_ShouldReturnBadRequest_NotExisitingUser()
        {
            Athrz();
            var addUserDto = new TeamUserDto
            {
                ProjectId = 1,
                UserId = 5,
                RoleId = 3,
            };

            var content = JsonContent.Create(addUserDto);

            var response = await _client.PutAsync("api/project/change-role", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        }
        [Fact]
        public async Task ChangeRole_ShouldReturnUnauthorized()
        {
            Athrz();
            var addUserDto = new TeamUserDto
            {
                ProjectId = 2,
                UserId = 2,
                RoleId = 3,
            };

            var content = JsonContent.Create(addUserDto);

            var response = await _client.PutAsync("api/project/change-role", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        }
        [Fact]
        public async Task AddStatus_ShouldReturnOk()
        {
            Athrz();
            var projectStatusDto = new ProjectStatusDto
            {
                ProjectId = 1,
                StatusId = 2
            };


            var response = await _client.PostAsJsonAsync("api/project/add-status", projectStatusDto);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task AddStatus_ShouldReturnBadRequest_NotExisitngProject()
        {
            Athrz();
            var projectStatusDto = new ProjectStatusDto
            {
                ProjectId = 3,
                StatusId = 2
            };


            var response = await _client.PostAsJsonAsync("api/project/add-status", projectStatusDto);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task AddStatus_ShouldReturnBadRequest_NotExisitngStatus()
        {
            Athrz();
            var projectStatusDto = new ProjectStatusDto
            {
                ProjectId = 1,
                StatusId = 8
            };


            var response = await _client.PostAsJsonAsync("api/project/add-status", projectStatusDto);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task AddStatus_ShouldReturnUnauthorized()
        {
            var projectStatusDto = new ProjectStatusDto
            {
                ProjectId = 1,
                StatusId = 2
            };


            var response = await _client.PostAsJsonAsync("api/project/add-status", projectStatusDto);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        [Fact]
        public async Task GetAllProjectWithPublicRecruitment_ShouldReturnOk()
        {
            Athrz();
            var response = await _client.GetAsync("api/project/projects-with-public-recruitment");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task GetAllProjectWithPublicRecruitment_ShouldReturnUnauthorized()
        {
            var response = await _client.GetAsync("api/project/projects-with-public-recruitment");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        
        
      
        
        [Fact]
        public async Task GetProjectsByTech_ShouldReturnOk()
        {
            Athrz();

            int techId = 1;


            var response = await _client.GetAsync($"api/project/search-by-tech/{techId}");


            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task GetProjectsByTech_ShouldReturnUnauthorized()
        {

            int techId = 1;


            var response = await _client.GetAsync($"api/project/search-by-tech/{techId}");


            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        [Fact]
        public async Task ApplyUserToProject_ReturnsOk()
        {
            Athrz();
            var applyToProjectDto = new ApplyToProjectDto
            {
                ProjectId = 1
            };
            var response = await _client.PostAsJsonAsync("api/project/apply", applyToProjectDto);


            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task ApplyUserToProject_ReturnsUnauthorized()
        {

            var applyToProjectDto = new ApplyToProjectDto
            {
                ProjectId = 2

            };
            var response = await _client.PostAsJsonAsync("api/project/apply", applyToProjectDto);


            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        [Fact]
        public async Task ApplyUserToProject_ReturnsBadRequest()
        {
            Athrz();
            var applyToProjectDto = new ApplyToProjectDto
            {
                
            };
            var response = await _client.PostAsJsonAsync("api/project/apply", applyToProjectDto);


            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        


    }
}