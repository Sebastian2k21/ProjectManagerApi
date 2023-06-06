using ProjectManagerApi.Data.Models;
using ProjectManagerApi.Data.Repositories;
using ProjectManagerApi.Exceptions;
using ProjectManagerApi.Extensions;

namespace ProjectManagerApi.Services
{
    public class ProjectService : IProjectService
    {
        private const string LeaderRoleName = "Leader";
        private const string CreatedStatusName = "Created";

        private readonly IBaseRepository<Project, int> projectRepository;
        private readonly IBaseRepository<User, int> userRepository;
        private readonly IBaseRepository<Tech, int> technologyRepsitory;
        private readonly IBaseRepository<Language, int> languageRepository;
        private readonly IBaseRepository<Team, int> teamRepository;
        private readonly IBaseRepository<TeamUser, (int userId, int teamId, int RoleId)> teamUserRepository;
        private readonly IBaseRepository<Role, int> roleRepository;
        private readonly IBaseRepository<Status, int> statusRepository;
        private readonly IBaseRepository<ProjectStatus, int> projectStatusRepository;

        public ProjectService(
            IBaseRepository<Project, int> projectRepository,
            IBaseRepository<User, int> userRepository,
            IBaseRepository<Tech, int> technologyRepsitory,
            IBaseRepository<Language, int> languageRepository,
            IBaseRepository<Team, int> teamRepository,
            IBaseRepository<TeamUser, (int userId, int teamId, int RoleId)> teamUserRepository,
            IBaseRepository<Role, int> roleRepository,
            IBaseRepository<Status, int> statusRepository,
            IBaseRepository<ProjectStatus, int> projectStatusRepository)
        {
            this.projectRepository = projectRepository;
            this.userRepository = userRepository;
            this.technologyRepsitory = technologyRepsitory;
            this.languageRepository = languageRepository;
            this.teamRepository = teamRepository;
            this.teamUserRepository = teamUserRepository;
            this.roleRepository = roleRepository;
            this.statusRepository = statusRepository;
            this.projectStatusRepository = projectStatusRepository;
        }

        public async Task<List<Project>> GetAllProjects()
        {
            return await projectRepository.GetAll();
        }

        public async Task<Project> CreateProject(int userId, Project project, List<int> technologies, List<int> languages)
        {
            var user = await GetUser(userId);

            var projectTechnologies = await technologyRepsitory.GetCollectionFromDB(technologies, "Technology not found");
            var projectLanguages = await languageRepository.GetCollectionFromDB(languages, "Language not found");

            var team = new Team { Name = project.Name + " - team" };

            var role = await roleRepository.FindFirst(role => role.Name == LeaderRoleName) ?? throw new InvalidItemException($"Role '{LeaderRoleName}' not found");

            var teamUser = new TeamUser
            {
                Role = role,
                Team = team,
                User = user
            };

            project.Team = team;
            project.Technologies.AddRange(projectTechnologies);
            project.Languages.AddRange(projectLanguages);

            var status = await statusRepository.FindFirst(status => status.Name == CreatedStatusName) ?? throw new InvalidItemException($"Status '{CreatedStatusName}' not found");

            await teamRepository.Add(team);
            await teamUserRepository.Add(teamUser);
            await projectRepository.Add(project);

            var projectStatus = new ProjectStatus
            {
                ProjectId = project.ProjectId,
                StatusId = status.StatusId
            };
            await projectStatusRepository.Add(projectStatus);

            return project;
        }

        public async Task AddUserToProject(int projectId, int leaderId, int userId, int roleId)
        {
            var leader = await GetUser(leaderId, errorMessage: "Leader not found");
            var user = await GetUser(userId);
            var project = await GetProject(projectId);

            if (!await IsLeaderOfProject(leaderId, project))
            {
                throw new InvalidItemException("User is not a leader of this project");
            }

            if (await teamUserRepository.FindFirst(x => x.UserId == userId && x.TeamId == project.TeamId) != null)
            {
                throw new InvalidItemException("User is already in this project");
            }

            var teamUser = new TeamUser
            {
                RoleId = roleId,
                TeamId = project.TeamId,
                UserId = userId
            };
            await teamUserRepository.Add(teamUser);
        }

        public async Task ChangeUserRole(int projectId, int leaderId, int userId, int roleId)
        {
            var leader = await GetUser(leaderId, errorMessage: "Leader not found");
            var user = await GetUser(userId);
            var project = await GetProject(projectId);

            if (!await IsLeaderOfProject(leaderId, project))
            {
                throw new InvalidItemException("User is not a leader of this project");
            }

            var teamUser = await teamUserRepository.FindFirst(x => x.UserId == userId && x.TeamId == project.TeamId);
            if (teamUser == null)
            {
                throw new InvalidItemException("User is not in this project");
            }

            if (teamUser.RoleId == roleId)
            {
                throw new InvalidItemException("User already has this role");
            }

            await teamUserRepository.Delete((teamUser.UserId.Value, teamUser.TeamId.Value, teamUser.RoleId.Value));
            await teamUserRepository.Add(new TeamUser
            {
                RoleId = roleId,
                UserId = userId,
                TeamId = project.TeamId
            });
        }

        public async Task SetProjectStatus(int projectId, int leaderId, int statusId)
        {
            var leader = await GetUser(leaderId, errorMessage: "Leader not found");
            var project = await GetProject(projectId);
            var status = await GetStatus(statusId);

            if (!await IsLeaderOfProject(leaderId, project))
            {
                throw new InvalidItemException("User is not a leader of this project");
            }

            await projectStatusRepository.Add(new ProjectStatus
            {
                ProjectId = projectId,
                StatusId = statusId
            });
        }

        private async Task<User> GetUser(int id, string? errorMessage = null) => await userRepository.Get(id) ?? throw new InvalidItemException(errorMessage ?? "User not found");

        private async Task<Project> GetProject(int id) => await projectRepository.Get(id) ?? throw new InvalidItemException("Project not found");
        private async Task<Status> GetStatus(int id) => await statusRepository.Get(id) ?? throw new InvalidItemException("Status not found");

        private async Task<bool> IsLeaderOfProject(int userId, Project project)
        {
            var leaderRole = await roleRepository.FindFirst(role => role.Name == LeaderRoleName) ?? throw new InvalidItemException($"Role '{LeaderRoleName}' not found");
            var teamLeader = await teamUserRepository.FindFirst(x => x.RoleId == leaderRole.RoleId && x.TeamId == project.TeamId) ?? throw new InvalidItemException("Team leader not found");
            return teamLeader.UserId == userId;
        }

        public async Task<IEnumerable<Project>> GetAllProjectWithPrivateRecruitment()
        {
            var projects = await projectRepository.GetAll();

            var pwpr = projects.Where(p => p.PrivateRecruitment == false).ToList();

            return pwpr;
        }

        public async Task<IEnumerable<Project>> GetProjectsByLanguage(int langId)
        {
            var allprojects = await projectRepository.GetAll();

            var language = (await languageRepository.GetAll()).Where(l => l.LanguageId == langId).FirstOrDefault();

            if (language != null)
            {
                var filteredprojects = allprojects.Where(p => p.Languages.Contains(language)).ToList();
                return filteredprojects;
            }

            else
            {
                throw new NullReferenceException("There aren't any projects with this language.");
            }

        }

        public async Task<IEnumerable<Project>> GetProjectsByTech(int techId)
        {
            var allprojects = await projectRepository.GetAll();

            var technology = (await technologyRepsitory.GetAll()).Where(l => l.TechId == techId).FirstOrDefault();

            if (technology != null)
            {
                var filteredprojects = allprojects.Where(p => p.Technologies.Contains(technology)).ToList();
                return filteredprojects;
            }

            else
            {
                throw new NullReferenceException("There aren't any projects with this technology.");
            }

        }

        public Task<Project> UpdateProject(int userId, Project project)
        {
            throw new NotImplementedException();
        }

        public async Task ApplyUserToProject(int userId, int projectId)
        {
            var user = await GetUser(userId);
            var project = await GetProject(projectId);
            if (project.PrivateRecruitment)
            {
                throw new InvalidItemException("Project has private recruitment.");
            }
            if (project.Applicants.Contains(user))
            {
                throw new InvalidItemException("User already applied to this project.");
            }
            if (await teamUserRepository.FindFirst(x => x.UserId == userId && x.TeamId == project.TeamId) != null)
            {
                throw new InvalidItemException("User is already in this project");
            }
            project.Applicants.Add(user);
            await projectRepository.Update(project);
        }

        public async Task<Project> GetProjectById(int projectId)
        {
            return await GetProject(projectId);
        }

        public async Task<IEnumerable<User>> GetApplicants(int leaderId, int projectId)
        {
            var project = await GetProject(projectId);
            if (!await IsLeaderOfProject(leaderId, project))
            {
                throw new InvalidItemException("User is not a leader of this project");
            }
            return project.Applicants;
        }
    }
}
