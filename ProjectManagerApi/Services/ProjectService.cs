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

            var role = await roleRepository.FindFirst(role => role.Name == LeaderRoleName) ?? throw new InvalidItemIdException($"Role '{LeaderRoleName}' not found");

            var teamUser = new TeamUser
            {
                Role = role,
                Team = team,
                User = user
            };

            project.Team = team;
            project.Technologies.AddRange(projectTechnologies);
            project.Languages.AddRange(projectLanguages);

            var status = await statusRepository.FindFirst(status => status.Name == CreatedStatusName) ?? throw new InvalidItemIdException($"Status '{CreatedStatusName}' not found");

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
            var role = await roleRepository.Get(roleId) ?? throw new InvalidItemIdException("Role not found");

            var leaderRole = await roleRepository.FindFirst(role => role.Name == LeaderRoleName) ?? throw new InvalidItemIdException($"Role '{LeaderRoleName}' not found");
            var teamLeader = await teamUserRepository.FindFirst(x => x.RoleId == leaderRole.RoleId && x.TeamId == project.TeamId) ?? throw new InvalidItemIdException("Team leader not found");

            if (teamLeader.UserId != leaderId)
            {
                throw new InvalidItemIdException("User is not a leader of this project");
            }

            if(await teamUserRepository.FindFirst(x => x.UserId == userId && x.TeamId == project.TeamId) != null)
            {
                throw new InvalidItemIdException("User is already in this project");
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
            var role = await roleRepository.Get(roleId) ?? throw new InvalidItemIdException("Role not found");

            var leaderRole = await roleRepository.FindFirst(role => role.Name == LeaderRoleName) ?? throw new InvalidItemIdException($"Role '{LeaderRoleName}' not found");
            var teamLeader = await teamUserRepository.FindFirst(x => x.RoleId == leaderRole.RoleId && x.TeamId == project.TeamId) ?? throw new InvalidItemIdException("Team leader not found");

            if (teamLeader.UserId != leaderId)
            {
                throw new InvalidItemIdException("User is not a leader of this project");
            }

            var teamUser = await teamUserRepository.FindFirst(x => x.UserId == userId && x.TeamId == project.TeamId);
            if (teamUser == null)
            {
                throw new InvalidItemIdException("User is not in this project");
            }

            if(teamUser.RoleId == roleId)
            {
                throw new InvalidItemIdException("User already has this role");
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

            var leaderRole = await roleRepository.FindFirst(role => role.Name == LeaderRoleName) ?? throw new InvalidItemIdException($"Role '{LeaderRoleName}' not found");
            var teamLeader = await teamUserRepository.FindFirst(x => x.RoleId == leaderRole.RoleId && x.TeamId == project.TeamId) ?? throw new InvalidItemIdException("Team leader not found");

            if (teamLeader.UserId != leaderId)
            {
                throw new InvalidItemIdException("User is not a leader of this project");
            }

            await projectStatusRepository.Add(new ProjectStatus
            {
                ProjectId = projectId,
                StatusId = statusId
            });
        }

        private async Task<User> GetUser(int id, string? errorMessage = null) => await userRepository.Get(id) ?? throw new InvalidItemIdException(errorMessage ?? "User not found");

        private async Task<Project> GetProject(int id) => await projectRepository.Get(id) ?? throw new InvalidItemIdException("Project not found");
        private async Task<Status> GetStatus(int id) => await statusRepository.Get(id) ?? throw new InvalidItemIdException("Status not found");
    }
}
